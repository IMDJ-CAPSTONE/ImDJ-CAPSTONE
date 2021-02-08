/*
 *  FILE          :	PhotonController.cs
 *  PROJECT       :	ImDJ Capstone Project 
 *  PROGRAMMER    :	Michael Hilts - 5377643
 *  FIRST VERSION :	Feb 6, 2021
 *  DESCRIPTION   : This file contains the PhotonController class which is responsible for all
 *                  interaction with the PhotonNetwork.
 */

#region Resources

using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

#endregion

public class PhotonController : MonoBehaviourPunCallbacks
{
    #region Fields

    public static PhotonController Instance = null;     // Instance of this class to allow access without refeence

    private UIController ui;                            // the ui

    #endregion

    #region MonoBehaviour Callbacks

    /*
     * METHOD     : Awake()
     * DESCRIPTION: Called when the scene is loaded. This will set the instance and
     *              get reference to the UIController.
     * PARAMETERS : 
     *      VOID
     * RETURNS    : 
     *      VOID
     */

    void Awake()
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
    }

    /*
     * METHOD     : OnApplicationQuit()
     * DESCRIPTION: Called when the application is quit. Will disconnect from the servers.
     * PARAMETERS : 
     *      VOID
     * RETURNS    : 
     *      VOID
     */

    private void OnApplicationQuit()
    {
        Disconnect();
    }

    #endregion

    #region PUN Callbacks

    /*
     * METHOD     : OnConnectedToMaster()
     * DESCRIPTION: Called once the application has connected to the Photon Master Server.
     * PARAMETERS : 
     *      VOID
     * RETURNS    : 
     *      VOID
     */

    public override void  OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        ui.DebugText = "Connected to Photon Server.";
    }

    /*
     * METHOD     : OnDisonnected()
     * DESCRIPTION: Called once the application has disconnected from Photon.
     * PARAMETERS : 
     *      VOID
     * RETURNS    : 
     *      VOID
     */

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        ui.DebugText = "Disconnected from Photon Servers.";
        ui.Quit();
    }

    #endregion

    #region Public Methods

    /*
     * METHOD     : Connect()
     * DESCRIPTION: This method may be called to connect to the Photon Master Server.
     * PARAMETERS : 
     *      VOID
     * RETURNS    : 
     *      VOID
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

    /*
     * METHOD     : Connect()
     * DESCRIPTION: This method may be called to disconnect from the Photon Master Server.
     * PARAMETERS : 
     *      VOID
     * RETURNS    : 
     *      VOID
     */

    public void Disconnect()
    {
        if (PhotonNetwork.IsConnected)
        {
            ui.DebugText = "Disconnecting from Photon Server...";
            PhotonNetwork.Disconnect();
        }
    }

    #endregion
}
