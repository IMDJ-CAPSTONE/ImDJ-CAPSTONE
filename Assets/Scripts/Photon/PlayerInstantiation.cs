using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInstantiation : MonoBehaviour, IOnEventCallback
{
    private const int instantiationEventCode = 0;

    public Color playerOneColor;
    public Color playerTwoColor;

    public GameObject localPrefab;
    public GameObject remotePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player;

        player = Instantiate(localPrefab, new Vector3(Random.Range(1f, 5f), 1.5f, Random.Range(1f, 5f)), localPrefab.transform.rotation);
        PhotonView photonView = player.GetComponent<PhotonView>();
        GameObject.FindGameObjectWithTag("Voice").GetComponent<PhotonVoiceNetwork>().PrimaryRecorder = player.GetComponent<Recorder>();
        player.GetComponent<PhotonVoiceView>().RecorderInUse = player.GetComponent<Recorder>();

        if (PhotonNetwork.AllocateViewID(photonView))
        {
            photonView.OwnershipTransfer = OwnershipOption.Takeover;
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);

            object[] data = { player.transform.position, player.transform.rotation, photonView.ViewID };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.Others,
                CachingOption = EventCaching.AddToRoomCache
            };

            if (PhotonNetwork.IsMasterClient)
            {
                player.gameObject.GetComponent<MeshRenderer>().material.color = playerOneColor;
            }
            else
            {
                player.gameObject.GetComponent<MeshRenderer>().material.color = playerTwoColor;
            }

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

            Vector3 pos = (Vector3)data[0];
            Quaternion rot = (Quaternion)data[1];
            int viewID = (int)data[2];

            GameObject player = Instantiate(remotePrefab, pos, rot);
            PhotonView photonView = player.GetComponent<PhotonView>();
            photonView.ViewID = viewID;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PhotonVoiceView>().SpeakerInUse = player.GetComponent<Speaker>();

            if (PhotonNetwork.IsMasterClient)
            {
                player.gameObject.GetComponent<MeshRenderer>().material.color = playerTwoColor;
            }
            else
            {
                player.gameObject.GetComponent<MeshRenderer>().material.color = playerOneColor;
            }
        }
    }
}
