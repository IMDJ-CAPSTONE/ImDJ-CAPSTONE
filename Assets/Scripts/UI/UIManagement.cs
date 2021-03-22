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

    public UserInstantiation userInst;

    public GameObject votingSystem;

    public GameObject AllUsersMenuResource;
    public GameObject DesktopVotingUIResource;
    
    public GameObject PerformerPollingUIResource;
    private GameObject PerformerPollingUIResourceButton;

    public int perfActorNum;

    private Dictionary<string, GameObject> UIContainers = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    public void StartFromScenarioManager()
    {
        UIContainers.Add("UserMenu", Instantiate(AllUsersMenuResource));
        UIContainers["UserMenu"].transform.SetParent(gameObject.transform);
        view = gameObject.GetComponent<PhotonView>();
        if ((UserType)PhotonNetwork.LocalPlayer.CustomProperties["Type"] == UserType.Performer)
        {
            UIContainers.Add("PerformerUI", Instantiate(PerformerPollingUIResource));
            UIContainers["PerformerUI"].transform.SetParent(gameObject.transform);
            PerformerPollingUIResourceButton = UIContainers["PerformerUI"].GetComponent<PerformerUserUIController>().NewPollButton;
            UIContainers["PerformerUI"].GetComponent<PerformerUserUIController>().VotingSystem = this.votingSystem;
            PerformerPollingUIResourceButton.GetComponent<LeanButton>().OnClick.AddListener(VotingShownForDesktopRPC);
            perfActorNum = PhotonNetwork.LocalPlayer.ActorNumber;
            view.RPC("SendActorNum", RpcTarget.OthersBuffered, perfActorNum);
        }
        
    }

    [PunRPC]
    public void SendActorNum(int _perfActorNum)
    {
        perfActorNum = _perfActorNum;
    }
    

    public void VotingShownForDesktopRPC()
    {
        view.RPC("DisplayVotingPanelRPC", RpcTarget.OthersBuffered);
    }

    [PunRPC]
    private void DisplayVotingPanelRPC()
    {
        if ((UserType)PhotonNetwork.LocalPlayer.CustomProperties["Type"] == UserType.Desktop)
        {
            UIContainers.Add("DesktopUI", Instantiate(DesktopVotingUIResource));
            UIContainers["DesktopUI"].transform.SetParent(gameObject.transform);
            UIContainers["DesktopUI"].GetComponent<DesktopUserVotingUI>().voteOption += (voteOption) => { view.RPC("VoteRPC", PhotonNetwork.CurrentRoom.GetPlayer(perfActorNum),voteOption); };
        }
    }

    [PunRPC]
    private void VoteRPC(int voteOption)
    {
        if ((UserType)PhotonNetwork.LocalPlayer.CustomProperties["Type"] == UserType.Performer)
        {
            votingSystem.GetComponent<VotingSystemController>().Voting(voteOption);
        }
    }
}
