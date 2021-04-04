/*
 *  FILE          :	WebcamClient.cs
 *  PROJECT       :	ImDJ Capstone Project 
 *  PROGRAMMER    :	Michael Hilts - 5377643
 *  FIRST VERSION :	Mar 27, 2021
 *  DESCRIPTION   : This file contains the WebcamClient class which is responsible for connecting to the 
 *					webcam server and then receiving frames and displaying them to the in scene canvas.
 */

#region Resources

using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class WebcamClient : MonoBehaviour, IOnEventCallback
{
	#region Fields

	private TcpClient client;			// the client which will connect to the server
	private Thread readThread;			// the thread for reading from the server

	private Texture2D camTexture;		// the texture to display images on
	private RawImage image;				// the image of the in scene canvas
	private volatile byte[] data;		// the data from the server 
	
	private volatile bool loadImage;    // flag whether or not to apply data to the texture

	#endregion

	#region MonoBehaviour Callbacks

	/*
	 * METHOD     : Start()
	 * DESCRIPTION: Called before the first frame. This will ensure it is not the performer
	 *				user, instantiate the client, and set the load image flag to false.
	 * PARAMETERS : 
	 *      VOID
	 * RETURNS    : 
	 *      VOID
	 */

	void Start()
    {
		if ((UserInstantiation.UserType)PhotonNetwork.LocalPlayer.CustomProperties["Type"] == UserInstantiation.UserType.Performer)
		{
			Destroy(gameObject);
			return;
		}

		client = new TcpClient();
		loadImage = false;		
	}

	/*
	 * METHOD     : Update()
	 * DESCRIPTION: Called once per frame. If there is data to load, then apply it to the texture.
	 * PARAMETERS : 
	 *      VOID
	 * RETURNS    : 
	 *      VOID
	 */

	private void Update()
	{
		if (loadImage)
		{
			ImageConversion.LoadImage(camTexture, data);
			camTexture.Apply();
			loadImage = false;
		}
	}

	/*
	 * METHOD     : OnEnable()
	 * DESCRIPTION: Used to hook this up to photon network callbacks.
	 * PARAMETERS : 
	 *      VOID
	 * RETURNS    : 
	 *      VOID
	 */

	private void OnEnable()
	{
		PhotonNetwork.AddCallbackTarget(this);
	}

	/*
	 * METHOD     : OnDisable()
	 * DESCRIPTION: Used to unhook this from photon network callbacks.
	 * PARAMETERS : 
	 *      VOID
	 * RETURNS    : 
	 *      VOID
	 */

	private void OnDisable()
	{
		PhotonNetwork.RemoveCallbackTarget(this);
	}

	/*
	 * METHOD     : OnDestroy()
	 * DESCRIPTION: Called when this is destroyed. Will clean up connections
	 *				and threads.
	 * PARAMETERS : 
	 *      VOID
	 * RETURNS    : 
	 *      VOID
	 */

	private void OnDestroy()
	{
		CleanUp();
	}

	/*
	 * METHOD     : OnApplicationQuit()
	 * DESCRIPTION: Called when this application closes. Will clean up connections
	 *				and threads.
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

	/*
	 * METHOD     : ClientThread()
	 * DESCRIPTION: This will read the size of the frame in when data is available then
	 *				use this to read the frame into the data, and if the read was successful
	 *				it will set the load image flag signifying the data is ready.
	 * PARAMETERS : 
	 *      VOID
	 * RETURNS    : 
	 *      VOID
	 */

	private void ClientThread()
	{
		NetworkStream stream = client.GetStream();
		byte[] sizeBuffer = new byte[sizeof(int)];
		int bytesRead;

		while (client.Connected)
		{
			if (stream.DataAvailable)
			{
				bytesRead = stream.Read(sizeBuffer, 0, sizeBuffer.Length);
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse(sizeBuffer);
				}
				int frameSize = BitConverter.ToInt32(sizeBuffer, 0);
				if (frameSize > 0)
				{
					data = new byte[frameSize];
					bytesRead = stream.Read(data, 0, data.Length);

					if (bytesRead == frameSize)
					{
						loadImage = true;
					}
				}
			}
		}
	}

	/*
	 * METHOD     : CleanUp()
	 * DESCRIPTION: This will close the connection and abort the client thread.
	 * PARAMETERS : 
	 *      VOID
	 * RETURNS    : 
	 *      VOID
	 */

	private void CleanUp()
	{
		if (client != null)
		{
			client.Close();
			client = null;
		}

		if (readThread != null)
		{
			readThread.Abort();
			readThread = null;
		}
	}

	#endregion

	#region IOnEventCallback Implementation

	/*
	 * METHOD     : OnEvent()
	 * DESCRIPTION: This method is called when a photon event is fired. If it is the
	 *				webcam event it will read the hostname and port and use them to
	 *				connect to the server. It will also read the height and width of the images
	 *				to set up the texture to display them.
	 * PARAMETERS : 
	 *      VOID
	 * RETURNS    : 
	 *      VOID
	 */

	public void OnEvent(EventData photonEvent)
	{
		// if this is the instantiation event
		if (photonEvent.Code == WebcamServer.WebcamEventCode)
		{
			// get the data sent with the event and parse it
			object[] data = photonEvent.CustomData as object[];
			string hostName = (string)data[0];
			int port = (int)data[1];
			int height = (int)data[2];
			int width = (int)data[3];

			// set up the texture to display the images received
			image = GameObject.FindGameObjectWithTag("Webcam").GetComponent<RawImage>();
			camTexture = new Texture2D(width, height);
			image.texture = camTexture;
			image.material.mainTexture = camTexture;

			// connect to the server and start the client thread
			client.Connect(hostName, port);
			readThread = new Thread(ClientThread);
			readThread.Start();
		}
	}

	#endregion
}
