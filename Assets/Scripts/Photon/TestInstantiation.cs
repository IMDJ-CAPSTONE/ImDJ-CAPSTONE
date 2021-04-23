/*! @file       : 	TestInstantiation.cs
*   @author     : 	Michael Hilts - 5377643
*   @date       : 	2021-02-20
*   @brief      : 	This file contains the TestInstantiation class which is responsible for
 *                  instantiating both players and the ico sphere on the network and
 *                  ensuring photon view ID's are set appropriately across all clients.
*/

#region Resources

using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion Resources

/*! <summary>
*  Contains the logic for the UI of the Performer, the code for all the buttons they can press
*  </summary>
*/
public class TestInstantiation : MonoBehaviourPunCallbacks, IOnEventCallback
{
    #region Constants

    private const int instantiationEventCode = 0;

    #endregion Constants

    #region Fields

    [SerializeField]
    [Tooltip("The prefab that will be sending the audio.")]
    private GameObject speaker;

    [SerializeField]
    [Tooltip("The prefab that will receive the audio.")]
    private GameObject listener;

    [SerializeField]
    [Tooltip("The prefab for players that are just listening.")]
    private GameObject audience;

    [SerializeField]
    [Tooltip("The prefab for the ico sphere.")]
    private GameObject icoPrefab;

    [SerializeField]
    [Tooltip("The color of the player that is transmitting audio.")]
    private Color playerOneColor = Color.red;

    [SerializeField]
    [Tooltip("The color of the player that will listen.")]
    private Color playerTwoColor = Color.blue;

    private Dictionary<int, GameObject> players;    // to keep track player gameobjects associated with actor numbers

    #endregion Fields

    #region MonoBehaviour CallBacks

    /*! <summary>
     *  Called when the scene is loaded. This will instantiate the players dictionary.
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    private void Awake()
    {
        players = new Dictionary<int, GameObject>();
    }

    /*! <summary>
     *  Called when this script is enabled. Will add this script to the photon
     *  networks callback list and set the sceneLoaded event to OnSceneLoaded
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /*! <summary>
     *  Called when this script is disabled. Will remove this script from the photon
     *  networks callback list and remove OnSceneLoaded from the sceneLoaded event.
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /*! <summary>
     *  Called when this scene is laoded. It will instantiate the local player and
     *  raise a network event to have remote clients instantiate the player as well.
     *  </summary>
     *  <param name="scene">the scene that was loaded</param>
     *  <param name="mode">the mode the scene was loaded in</param>
     *  <returns>void</returns>
     */
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player;      // the local player that will be instantiated
        int icoViewID = -1;     // the viewID of the icosphere

        // if this is the first user connected, they will be the one to send audio
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            // instantiate the player as a speaker, and set the color appropriately
            player = Instantiate(speaker, Vector3.zero, Quaternion.identity);
            player.GetComponent<MeshRenderer>().material.color = playerOneColor;

            // instantiate the ico sphere on the network and get its view id
            GameObject ico = PhotonNetwork.Instantiate(icoPrefab.name, Vector3.zero, Quaternion.identity);
            icoViewID = ico.GetComponent<PhotonView>().ViewID;
        }

        // if this isnt the player to send audio, instantiate them as audience, and set color
        else
        {
            player = Instantiate(audience, new Vector3(2f, 0, 2f), Quaternion.identity);
            player.GetComponent<MeshRenderer>().material.color = playerTwoColor;
        }

        // add this player to the dictionary with the actor number as the key
        players.Add(PhotonNetwork.LocalPlayer.ActorNumber, player);

        // get photonview for the player and allocate a view ID
        PhotonView photonView = player.GetComponent<PhotonView>();
        if (PhotonNetwork.AllocateViewID(photonView))
        {
            // transfer ownership of this photon view to this player
            photonView.OwnershipTransfer = OwnershipOption.Takeover;
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);

            // set up data to send and event options
            object[] data = { PhotonNetwork.LocalPlayer.ActorNumber, photonView.ViewID, icoViewID };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.Others,               // send to all but this client
                CachingOption = EventCaching.AddToRoomCache     // cache so players entering room later will still get the event
            };

            // rais the photon network event
            PhotonNetwork.RaiseEvent(instantiationEventCode, data, raiseEventOptions, SendOptions.SendReliable);
        }
        else
        {
            Debug.LogError("Failed to allocate a ViewID.");
            Destroy(player);
        }
    }

    #endregion MonoBehaviour CallBacks

    #region Pun Callbacks

    /*! <summary>
     *  Called when a player leaves the room. Will ensure their player gameobject is removed on all other clients
     *  </summary>
     *  <param name="otherPlayer">the player who left.</param>
     *  <returns>void</returns>
     */
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        GameObject player;
        if (players.TryGetValue(otherPlayer.ActorNumber, out player))
        {
            Destroy(player);
        }
        players.Remove(otherPlayer.ActorNumber);
    }

    #endregion Pun Callbacks

    #region IOnEventCallback Implementation

    /*! <summary>
     *  Called when a photon network event is raised. If this is the instantiation event
     *  it will instantiate the player who sent the request on this client.
     *  </summary>
     *  <param name="phontonEvent">this object is used to check if its an instantiation event</param>
     *  <returns>void</returns>
     */
    public void OnEvent(EventData photonEvent)
    {
        // if this is the instantiation event
        if (photonEvent.Code == instantiationEventCode)
        {
            // get the data sent with the event and parse it
            object[] data = photonEvent.CustomData as object[];
            int actorNum = (int)data[0];
            int viewID = (int)data[1];
            int icoViewID = (int)data[2];

            GameObject player;  // the player that will be instantiated

            // if the actor who is sending audio raised the event
            if (actorNum == 1)
            {
                // instantiate their player as the listener who receives audio
                player = Instantiate(listener, Vector3.zero, Quaternion.identity);

                // set color and view id
                player.GetComponent<MeshRenderer>().material.color = playerOneColor;
                player.GetComponent<PhotonView>().ViewID = viewID;

                // if the icoViewID was sent
                if (icoViewID != -1)
                {
                    // find the gameobject and remove the ableton script
                    GameObject ico = PhotonView.Find(icoViewID).gameObject;
                    if (ico)
                    {
                        Destroy(ico.GetComponent<AbletonLinkTest2>());
                    }
                }
            }
            else  // if the actor who raised the event is just listening instantiate as audience and set color
            {
                player = Instantiate(audience, new Vector3(2f, 0f, 2f), Quaternion.identity);
                player.GetComponent<MeshRenderer>().material.color = playerTwoColor;
            }

            // add this player to the dictionary
            players.Add(actorNum, player);
        }
    }

    #endregion IOnEventCallback Implementation
}
