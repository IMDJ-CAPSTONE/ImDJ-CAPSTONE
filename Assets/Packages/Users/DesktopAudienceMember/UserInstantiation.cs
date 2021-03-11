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
    private GameObject audience;

    [SerializeField]
    [Tooltip("The list of audience members.")]
    private Dictionary<int, GameObject> players;

    [SerializeField]
    [Tooltip("The color of the player that is transmitting audio.")]
    private Color playerOneColor = Color.red;

    [SerializeField]
    [Tooltip("The color of the player that will listen.")]
    private Color playerTwoColor = Color.blue;

    public enum UserType : int
    {
        Desk = 1,
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
        GameObject player;
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            // instantiate the player as a speaker, and set the color appropriately
            player = Instantiate(speaker, Vector3.zero, Quaternion.identity);
            player.GetComponent<MeshRenderer>().material.color = playerOneColor;
        }
    }

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
            }

            // if the actor who raised the event is just listening instantiate as audience and set color
            else
            {
                player = Instantiate(audience, new Vector3(2f, 0f, 2f), Quaternion.identity);
                player.GetComponent<MeshRenderer>().material.color = playerTwoColor;
            }

            // add this player to the dictionary
            players.Add(actorNum, player);
        }
    }
}
