using Lean.Gui;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
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
        List<string> theOptions = new List<string>();
        for(int i =0; i<votingSystem.GetComponent<VotingSystemController>().options.Count; i++)
        {
            theOptions.Add(votingSystem.GetComponent<VotingSystemController>().options[i + 1].OptionName);
        }
        string[] arrTheOption = theOptions.ToArray();
        view.RPC("DisplayVotingPanelRPC", RpcTarget.OthersBuffered,
            arrTheOption, 
            votingSystem.GetComponent<VotingSystemController>().Question, 
            votingSystem.GetComponent<VotingSystemController>().totalOptions);
    }

    [PunRPC]
    private void DisplayVotingPanelRPC(string[] theOptions, string question, int totalOptions)
    {
        if ((UserType)PhotonNetwork.LocalPlayer.CustomProperties["Type"] == UserType.Desktop)
        {
            if (UIContainers.ContainsKey("DesktopUI") == true)
            {
                Destroy(UIContainers["DesktopUI"]);
                UIContainers.Remove("DesktopUI");
                UIContainers.Add("DesktopUI", Instantiate(DesktopVotingUIResource));
            }
            else //if (UIContainers.ContainsKey("DesktopUI") == false)
            {
                UIContainers.Add("DesktopUI", Instantiate(DesktopVotingUIResource));
            }

            UIContainers["DesktopUI"].GetComponent<DesktopUserVotingUI>().setupUI(totalOptions, theOptions, question);

            UIContainers["DesktopUI"].transform.SetParent(gameObject.transform);
            UIContainers["DesktopUI"].GetComponent<DesktopUserVotingUI>().voteOption += (voteOption) => 
                { view.RPC("VoteRPC", PhotonNetwork.CurrentRoom.GetPlayer(perfActorNum), voteOption); };
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
