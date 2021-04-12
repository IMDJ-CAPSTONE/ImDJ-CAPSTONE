using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UserInstantiation : MonoBehaviourPunCallbacks, IOnEventCallback
{

    private const int instantiationEventCode = 0;

    [SerializeField]
    [Tooltip("The prefab that will be sending the audio.")]
    private GameObject speaker;

    [SerializeField]
    [Tooltip("The prefab that will receive the audio.")]
    private GameObject listener;

    [SerializeField]
    [Tooltip("The prefab for players that are just listening.")]
    private GameObject audienceLocal;

	[SerializeField]
	[Tooltip("The prefab for players that are just listening.")]
	private GameObject audienceRemote;

	[SerializeField]
    [Tooltip("The list of audience members.")]
    private Dictionary<int, GameObject> players;

    public int perfActNum;

    [SerializeField]
    [Tooltip("The color of the player that is transmitting audio.")]
    private Color playerOneColor = Color.red;

    [SerializeField]
    [Tooltip("The color of the player that will listen.")]
    private Color playerTwoColor = Color.blue;

    public enum UserType : int
    {
        Desktop = 1,
        VR = 2,
        Performer = 3
    }

    // Start is called before the first frame update
    void Start()
    {
         
    }

    void Awake()
    {
        players = new Dictionary<int, GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        GameObject player = null;
        if ((UserType)PhotonNetwork.LocalPlayer.CustomProperties["Type"] == UserType.Performer)
        {
            // instantiate the player as a speaker, and set the color appropriately
            player = Instantiate(speaker, speaker.transform.position, Quaternion.identity);
            player.GetComponent<MeshRenderer>().material.color = playerOneColor;
            
        }
        else if ((UserType)PhotonNetwork.LocalPlayer.CustomProperties["Type"] == UserType.Desktop)
        {
            player = Instantiate(audienceLocal, audienceLocal.transform.position, Quaternion.identity);
            player.GetComponent<MeshRenderer>().material.color = playerTwoColor;
			GameObject cameraGO = Camera.main.gameObject;
			Vector3 eulers = cameraGO.transform.rotation.eulerAngles;
			eulers.y = 0f;
			cameraGO.transform.rotation = Quaternion.Euler(eulers);
			cameraGO.GetComponent<CameraController>().SetTarget(player);
		}
        //add else for VR for instantiation
        //else if ((UserType)PhotonNetwork.LocalPlayer.CustomProperties["Type"] == UserType.VR)
        //{
        //    player = Instantiate(audience, new Vector3(2f, 0, 2f), Quaternion.identity);
        //    player.GetComponent<MeshRenderer>().material.color = playerTwoColor;
        //}
        if(player != null){
            players.Add(PhotonNetwork.LocalPlayer.ActorNumber, player);
        }

        PhotonView photonView = player.GetComponent<PhotonView>();
        if (PhotonNetwork.AllocateViewID(photonView))
        {
            // transfer ownership of this photon view to this player
            photonView.OwnershipTransfer = OwnershipOption.Takeover;
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);

            // set up data to send and event options
            object[] data = { PhotonNetwork.LocalPlayer.CustomProperties["Type"], photonView.ViewID , PhotonNetwork.LocalPlayer.ActorNumber };
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

    public void OnEvent(EventData photonEvent)
    {
        // if this is the instantiation event
        if (photonEvent.Code == instantiationEventCode)
        {
            // get the data sent with the event and parse it
            object[] data = photonEvent.CustomData as object[];
            
            var thisUserType = (UserType)data[0];
            int viewID = (int)data[1];
            int actorNum = (int)data[2];

            GameObject player;  // the player that will be instantiated

            // if the actor who is sending audio raised the event
            if (thisUserType == UserType.Performer)
            {
                // instantiate their player as the listener who receives audio
                player = Instantiate(listener, listener.transform.position, Quaternion.identity);

                // set color and view id
                player.GetComponent<MeshRenderer>().material.color = playerOneColor;
                player.GetComponent<PhotonView>().ViewID = viewID;
            }

            // if the actor who raised the event is just listening instantiate as audience and set color
            else
            {
                player = Instantiate(audienceRemote, audienceRemote.transform.position, Quaternion.identity);
                player.GetComponent<MeshRenderer>().material.color = playerTwoColor;
				player.GetComponent<PhotonView>().ViewID = viewID;
			}

            // add this player to the dictionary
            players.Add(actorNum, player);
        }
    }
}
