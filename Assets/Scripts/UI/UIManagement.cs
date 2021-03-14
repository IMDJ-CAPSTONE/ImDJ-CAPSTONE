using Lean.Gui;
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

    public GameObject AllUsersMenuResource;
    public GameObject DesktopVotingUIResource;
    
    public GameObject PerformerPollingUIResource;
    private GameObject PerformerPollingUIResourceButton;

    private Dictionary<string, GameObject> UIContainers = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        UIContainers.Add("UserMenu", Instantiate(AllUsersMenuResource));
        UIContainers["UserMenu"].transform.SetParent(gameObject.transform);
        if ((UserType)PhotonNetwork.LocalPlayer.CustomProperties["Type"] == UserType.Performer)
        {
            UIContainers.Add("PerformerUI", Instantiate(PerformerPollingUIResource));
            UIContainers["PerformerUI"].transform.SetParent(gameObject.transform);
            PerformerPollingUIResourceButton = UIContainers["PerformerUI"].GetComponent<PerformerUserUIController>().button;
            PerformerPollingUIResourceButton.GetComponent<LeanButton>().OnClick.AddListener(VotingShownForDesktop);
        }
        view = gameObject.GetComponent<PhotonView>();
    }
    

    public void VotingShownForDesktop()
    {
        view.RPC("DisplayVotingPanel", RpcTarget.Others);
    }

    [PunRPC]
    private void DisplayVotingPanel()
    {
        if ((UserType)PhotonNetwork.LocalPlayer.CustomProperties["Type"] == UserType.Desktop)
        {
            
            UIContainers.Add("DesktopUI", Instantiate(DesktopVotingUIResource));
            UIContainers["DesktopUI"].transform.SetParent(gameObject.transform);
        }
    }
}
