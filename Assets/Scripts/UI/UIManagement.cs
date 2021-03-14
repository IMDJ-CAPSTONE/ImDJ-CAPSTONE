using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UserInstantiation;

public class UIManagement : MonoBehaviourPunCallbacks
{
    private PhotonView view;

    public GameObject DesktopVotingUIResource;
    
    public GameObject PerformerUIResource;

    private Dictionary<string, GameObject> UIContainers = new Dictionary<string, GameObject>();


    public GameObject button;
    public GameObject twitchPanel;

    // Start is called before the first frame update
    void Start()
    {
        if ((UserType)PhotonNetwork.LocalPlayer.CustomProperties["Type"] == UserType.Performer)
        {
            UIContainers.Add("PerformerUI", Instantiate(PerformerUIResource));
            UIContainers["PerformerUI"].transform.SetParent(gameObject.transform);
            button = UIContainers["PerformerUI"].GetComponent<PerformerUserUIController>().button;
            button.GetComponent<Button>().onClick.AddListener(VotingShownForDesktop);
        }

        view = gameObject.GetComponent<PhotonView>();
    }
    

    public void VotingShownForDesktop()
    {
        view.RPC("DisplayPanel", RpcTarget.Others);
    }

    [PunRPC]
    private void DisplayDesktopVotingPanel()
    {
        if ((UserType)PhotonNetwork.LocalPlayer.CustomProperties["Type"] == UserType.Desktop)
        {
            UIContainers.Add("DesktopUI", Instantiate(DesktopVotingUIResource));
            UIContainers["DesktopUI"].transform.SetParent(gameObject.transform);
        }
    }
}
