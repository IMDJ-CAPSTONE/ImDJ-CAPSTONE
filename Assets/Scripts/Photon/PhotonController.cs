/*! @file       : 	PhotonController.cs
*   @author     : 	Michael Hilts, Ivan Granic
*   @date       : 	2021-02-06
*   @brief      : 	This file contains the PhotonController class which is responsible for all
*                   interaction with the PhotonNetwork.
*/

#region Resources

using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

#endregion Resources

/*! <summary>
*  This file contains the PhotonController class which is responsible for all interaction with the PhotonNetwork.
*  </summary>
*/
public class PhotonController : MonoBehaviourPunCallbacks
{
    #region Fields

    public static PhotonController Instance = null;     // Instance of this class to allow access without refeence
    private UIController ui;                            // the ui

    #endregion Fields

    #region MonoBehaviour Callbacks

    /*! <summary>
     *  Called when the scene is loaded. This will set the instance and get reference to the UIController.
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError(
                "Duplicate PhotonController detected on GameObject " + gameObject.name + ". " +
                "Deleting duplicate now.");
            Destroy(this);
        }
        Instance = this;

        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UIController>();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    /*! <summary>
     *  Called when the application is quit. Will disconnect from the servers.
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    private void OnApplicationQuit()
    {
        Disconnect();
    }

    #endregion MonoBehaviour Callbacks

    #region PUN Callbacks

    /*! <summary>
     *  Called once the application has connected to the Photon Master Server.
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        ui.DebugText = "Connected to Photon Server. Joining a random room.";
        PhotonNetwork.JoinRandomRoom();
    }

    /*! <summary>
     *  Called once the application has disconnected from Photon.
     *  </summary>
     *  <param name="cause">The cause of the disconect</param>
     *  <returns>void</returns>
     */
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        ui.DebugText = "Disconnected from Photon Servers.";
        ui.Quit();
    }

    /*! <summary>
     *  Called once user has joined a room on the Photon Server.
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        ui.DebugText = "Joined room " + PhotonNetwork.CurrentRoom.Name + ".";
        PhotonNetwork.LoadLevel("VoiceTestScene");
    }

    /*! <summary>
     *  Called once user failed to join a random room.
     *  </summary>
     *  <param name="returnCode">return code from the server operation</param>
     *  <param name="message">debug messages for the error</param>
     *  <returns>void</returns>
     */
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        ui.DebugText = "Failed to join a random room. Creating a room.";
        PhotonNetwork.CreateRoom("VoiceTestRoom");
    }

    /*! <summary>
     *  Called once user has created a room on the photon server.
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        ui.DebugText = "Created room " + PhotonNetwork.CurrentRoom.Name + ".";
    }

    #endregion PUN Callbacks

    #region Public Methods

    /*! <summary>
     *  This method may be called to connect to the Photon Master Server.
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    public void Connect()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            ui.DebugText = "Already connected to Photon Server.";
        }
        else
        {
            ui.DebugText = "Connecting to Photon Server...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    /*! <summary>
     *  This method may be called to disconnect from the Photon Master Server.
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    public void Disconnect()
    {
        if (PhotonNetwork.IsConnected)
        {
            ui.DebugText = "Disconnecting from Photon Server...";
            PhotonNetwork.Disconnect();
        }
    }

    #endregion Public Methods
}
