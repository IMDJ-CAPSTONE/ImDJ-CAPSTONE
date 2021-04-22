/*
 *  FILE          :	WebcamServer.cs
 *  PROJECT       :	ImDJ Capstone Project 
 *  PROGRAMMER    :	Michael Hilts - 5377643
 *  FIRST VERSION :	Mar 27, 2021
 *  DESCRIPTION   : This file contains the WebcamServer class which is responsible for starting the
 *					webcam and sending the data to any clients that connect to it.
 */

#region Resources

using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class WebcamServer : MonoBehaviour
{
	#region Constants

	public static readonly byte WebcamEventCode = 1;

	#endregion

	#region Fields

	[Tooltip("The port to listen on.")]
	[SerializeField]
	private int port = 2626;
	private string address;							// the address the server can be reached 
	private TcpListener server;						// the server which will listen for clients
	private Thread connectionThread;				// the thread running the connection loop
	private List<Thread> threadList;				// a list of threads, one per client
	private List<TcpClient> clientList;             // a list of the connected clients
	private Thread updateCamThread;
	private WebCamTexture camTexture;				// the webcam texture used to capture webcam
	private Texture2D snapshot;						// the texture used to capture snapshots of webcam and convert to png
	private volatile byte[] data;					// the data containing the captured screenshot to send over the network
	private int height;								// heigh of the captured image
	private int width;								// width of the captured image
	private volatile bool CamUpdated = false;       // flag to determine whether or not to send an image

	#endregion

	#region MonoBehaviour Callbacks

	/* METHOD     : Start()
	 * DESCRIPTION: Called before the first frame. This will ensure it is the performer
	 *				user, initialize fields, start the webcam, send the webcam event to photon network
	 *				and start the webcam server.
	 * PARAMETERS : 
	 *      VOID
	 * RETURNS    : 
	 *      VOID
	 */
	void Start()
    {
		// If this isnt the performer, destroy the gameobject
		if ((UserInstantiation.UserType)PhotonNetwork.LocalPlayer.CustomProperties["Type"] != UserInstantiation.UserType.Performer)
		{
			Destroy(gameObject);
			return;
		}

		// initialize lists and get ip address
		threadList = new List<Thread>();
		clientList = new List<TcpClient>();
		address = "127.0.0.1";

		// start webcam and server to send to clients
		StartWebcam();
		RaiseWebcamEvent();
		StartServer();
		//updateCamThread = new Thread(camUpdate);
		//updateCamThread.Start();
	}

	/* METHOD     : Update()
	 * DESCRIPTION: Called once per frame. This will check if the camera updated this frame
	 *				and if so it will capture the image data.
	 * PARAMETERS : 
	 *      VOID
	 * RETURNS    : 
	 *      VOID
	 */
	private void Update()
	{
		CamUpdated = camTexture.didUpdateThisFrame;
		if (CamUpdated)
		{
			snapshot.SetPixels(camTexture.GetPixels());
			data = snapshot.GetRawTextureData();
		}
	}

	/* METHOD     : OnDestroy()
	 * DESCRIPTION: Called when this is destroyed. Will close and open connections and
	 *				abort any running threads and close the server.
	 * PARAMETERS : 
	 *      VOID
	 * RETURNS    : 
	 *      VOID
	 */
	private void OnDestroy()
	{
		CleanUp();
	}

	/* METHOD     : OnApplicationQuit()
	 * DESCRIPTION: Called when this is application is closed. Will close and open connections and
	 *				abort any running threads and close the server.
	 * PARAMETERS : 
	 *      VOID
	 * RETURNS    : 
	 *      VOID
	 */
	void OnApplicationQuit()
	{
		CleanUp();
	}

	#endregion

	#region Private Methods

	/* METHOD     : StartWebcam()
	 * DESCRIPTION: Used to start the webcam. It will instantiate a webcam texture, get reference to the
	 *				image on the canvas in scene and set the webcam texture to it, then play the camera.
	 *				Once running it will cache the height and width of the captured image and use it to
	 *				instantiate the snapshot which will be used to capture frames.
	 * PARAMETERS : 
	 *      VOID
	 * RETURNS    : 
	 *      VOID
	 */
	private void StartWebcam()
	{
		camTexture = new WebCamTexture();
		RawImage image = GameObject.FindGameObjectWithTag("Webcam").GetComponent<RawImage>();
		image.texture = camTexture;
		image.material.mainTexture = camTexture;
		camTexture.Play();
		height = camTexture.height;
		width = camTexture.width;
		snapshot = new Texture2D(width, height, TextureFormat.ARGB32, false);
	}

	/* METHOD     : RaiseWebcamEvent()
	 * DESCRIPTION: This is used to send an event to the photon network with the servers
	 *				ip and port, as well as the height and width of the image. Any client that
	 *				joins the room will get the event and use it to connect to this server.
	 * PARAMETERS : 
	 *      VOID
	 * RETURNS    : 
	 *      VOID
	 */
	private void RaiseWebcamEvent()
	{
		object[] data = { address, port, height, width };
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions
		{
			Receivers = ReceiverGroup.Others,               // send to all but this client
			CachingOption = EventCaching.AddToRoomCache     // cache so players entering room later will still get the event
		};
		PhotonNetwork.RaiseEvent(WebcamEventCode, data, raiseEventOptions, SendOptions.SendReliable);
	}

	/* METHOD     : StartServer()
	 * DESCRIPTION: This is used to instantiate the server and start it. It will then begin
	 *				the connection loop thread to asynchronously listen for clients.
	 * PARAMETERS : 
	 *      VOID
	 * RETURNS    : 
	 *      VOID
	 */
	private void StartServer()
	{
		server = new TcpListener(IPAddress.Parse(address), port);
		server.Start();
		Debug.Log(
				"[+] Server has started on " + address + ":" + port +
				".\n[+] Waiting for a connection...");

		connectionThread = new Thread(ConnectionLoop);
		connectionThread.Priority = System.Threading.ThreadPriority.Highest;
		connectionThread.Start();
	}

	/* METHOD     : ConnectionLoop()
	 * DESCRIPTION: This will run on a seperate thread and will listen for new clients who
	 *				connect to this server. For each client that connects it will begin a
	 *				client thread to handle sending them the frame data.
	 * PARAMETERS : 
	 *      VOID
	 * RETURNS    : 
	 *      VOID
	 */
	private void ConnectionLoop()
	{
		while (true) 
		{
			TcpClient client = server.AcceptTcpClient();
			clientList.Add(client);
			Thread thread = new Thread(ClientThread);
			threadList.Add(thread);
			Debug.Log("[+] A client has connected.");
			thread.Priority = System.Threading.ThreadPriority.Highest;
			thread.Start(client);
		}
	}

	/* METHOD     : ClientThread()
	 * DESCRIPTION: This method will send two messages to the connected client
	 *				for each webcam frame update. The first is the size of the image
	 *				captured and the second is the captured image as a byte array.
	 * PARAMETERS : 
	 *      object _data: the client that will be run on this thread.
	 * RETURNS    : 
	 *      VOID
	 */
	private void ClientThread(object _data)
	{
		TcpClient myClient = (TcpClient)_data;
		NetworkStream stream = myClient.GetStream();

		while (myClient.Connected)
		{
			if (stream.CanWrite && CamUpdated)
			{
				if (data != null)
				{
					int size = data.Length;
					byte[] sizeData = BitConverter.GetBytes(size);
					if (BitConverter.IsLittleEndian)
					{
						Array.Reverse(sizeData);
					}

					stream.Write(sizeData, 0, sizeData.Length);
					stream.Write(data, 0, size);
					data = null;
				}
			}
		}

		stream.Close();
	}

	/* METHOD     : CleanUp()
	 * DESCRIPTION: Will close and open connections and
	 *				abort any running threads and close the server.
	 * PARAMETERS : 
	 *      VOID
	 * RETURNS    : 
	 *      VOID
	 */
	private void CleanUp()
	{
		// abort any client threads
		if (threadList != null)
		{
			foreach (Thread current in threadList)
			{
				if (current.IsAlive)
				{
					current.Abort();
				}
			}
			threadList.Clear();
			threadList = null;
		}

		// abort the connection loop thread
		if (connectionThread != null && connectionThread.IsAlive)
		{
			connectionThread.Abort();
			connectionThread = null;
		}

		// close any open connections
		if (clientList != null)
		{
			foreach (TcpClient current in clientList)
			{
				current.Close();
			}
			clientList.Clear();
			clientList = null;
		}

		// shut down the server
		if (server != null)
		{
			server.Stop();
			server = null;
		}
	}

	#endregion
}
