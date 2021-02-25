using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestInstantiation : MonoBehaviourPunCallbacks, IOnEventCallback
{
    private const int instantiationEventCode = 0;

    public GameObject speaker;
    public GameObject listener;

    public GameObject audience;

    public Color playerOneColor = Color.red;
    public Color playerTwoColor = Color.blue;

    private Dictionary<int, GameObject> players;

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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        GameObject player;
        if (players.TryGetValue(otherPlayer.ActorNumber, out player))
        {
            Destroy(player);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player;

        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            player = Instantiate(speaker, Vector3.zero, Quaternion.identity);
            player.GetComponent<MeshRenderer>().material.color = playerOneColor;
        }
        else
        {
            player = Instantiate(audience, new Vector3(2f, 0, 2f), Quaternion.identity);
            player.GetComponent<MeshRenderer>().material.color = playerTwoColor;
        }

        players.Add(PhotonNetwork.LocalPlayer.ActorNumber, player);

        PhotonView photonView = player.GetComponent<PhotonView>();

        if (PhotonNetwork.AllocateViewID(photonView))
        {
            photonView.OwnershipTransfer = OwnershipOption.Takeover;
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);

            object[] data = { PhotonNetwork.LocalPlayer.ActorNumber, photonView.ViewID };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.Others,
                CachingOption = EventCaching.AddToRoomCache
            };

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
        if (photonEvent.Code == instantiationEventCode)
        {
            object[] data = photonEvent.CustomData as object[];

            int actorNum = (int)data[0];
            int viewID = (int)data[1];

            GameObject player;

            if (actorNum == 1)
            {
                player = Instantiate(listener, Vector3.zero, Quaternion.identity);
                player.GetComponent<MeshRenderer>().material.color = playerOneColor;
                player.GetComponent<PhotonView>().ViewID = viewID;
            }
            else
            {
                player = Instantiate(audience, new Vector3(2f, 0f, 2f), Quaternion.identity);
                player.GetComponent<MeshRenderer>().material.color = playerTwoColor;
            }

            players.Add(actorNum, player);
        }
    }
}

