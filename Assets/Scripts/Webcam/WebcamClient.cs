/*! @file       : 	WebcamClient.cs
*   @author     : 	Mike Hilts, Ivan Granic, Jason Kassies, Div Dankahara
*   @date       : 	2021-03-27
*   @brief      : 	This file contains the WebcamClient class and server functionality
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

#endregion Resources

/*! <summary>
*  This file contains the WebcamClient class which is responsible for connecting to the
*  webcam server and then receiving frames and displaying them to the in scene canvas.
*  </summary>
*/
public class WebcamClient : MonoBehaviour, IOnEventCallback
{
    #region Fields

    private TcpClient client;           // the client which will connect to the server
    private Thread readThread;          // the thread for reading from the server
    private Texture2D camTexture;       // the texture to display images on
    private RawImage image;             // the image of the in scene canvas
    private byte[] data;                // the data from the server
    private volatile bool loadImage;    // flag whether or not to apply data to the texture

    #endregion Fields

    #region MonoBehaviour Callbacks

    /*! <summary>
     *  Called before the first frame. This will ensure it is not the performer user,
	 *	instantiate the client, and set the load image flag to false.
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    private void Start()
    {
        if ((UserInstantiation.UserType)PhotonNetwork.LocalPlayer.CustomProperties["Type"] == UserInstantiation.UserType.Performer)
        {
            Destroy(gameObject);
            return;
        }

        client = new TcpClient();
        loadImage = false;
    }

    /*! <summary>
     *  Called once per frame. If there is data to load, then apply it to the texture.
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    private void Update()
    {
        if (loadImage)
        {
            camTexture.LoadRawTextureData(data);
            camTexture.Apply();
            loadImage = false;
        }
    }

    /*! <summary>
     *  Used to hook this up to photon network callbacks.
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    /*! <summary>
     *  Used to unhook this from photon network callbacks.
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    /*! <summary>
     *  Called when this is destroyed. Will clean up connections and threads.
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    private void OnDestroy()
    {
        CleanUp();
    }

    /*! <summary>
     *  Called when this application closes. Will clean up connections and threads.
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    private void OnApplicationQuit()
    {
        CleanUp();
    }

    #endregion MonoBehaviour Callbacks

    #region Private Methods

    /*! <summary>
     *  This will read the size of the frame in when data is available then use this to read
     *  the frame into the data, and if the read was successful it will set the load
     *  image flag signifying the data is ready.
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
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

    /*! <summary>
     *  This will close the connection and abort the client thread.
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
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

    #endregion Private Methods

    #region IOnEventCallback Implementation

    /*! <summary>
     *  This method is called when a photon event is fired. If it is the webcam event it will
     *  read the hostname and port and use them to connect to the server.
     *  It will also read the height and width of the images to set up the texture to display them.
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
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
            camTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
            image.texture = camTexture;
            image.material.mainTexture = camTexture;

            // connect to the server and start the client thread
            client.Connect(hostName, port);
            readThread = new Thread(ClientThread);
            readThread.Priority = System.Threading.ThreadPriority.Highest;
            readThread.Start();
        }
    }

    #endregion IOnEventCallback Implementation
}
