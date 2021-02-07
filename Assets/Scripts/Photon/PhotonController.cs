

using Photon.Pun;
using UnityEngine;

public class PhotonController : MonoBehaviourPunCallbacks
{
    public static PhotonController Instance = null;

    private UIController ui;

    // Start is called before the first frame update
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

    public override void  OnConnectedToMaster()
    {
        ui.DebugText = "Connected to Photon Master Server.";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Connect()
    {
        ui.DebugText = "Connecting to Photon Master Server...";
        PhotonNetwork.ConnectUsingSettings();
    }
}
