/*  FILE          : 	UIManagement.cs
*   PROJECT       : 	PROG3221 - Capstone
*   PROGRAMMER    : 	Ivan Granic, Jason Kassies, Div Dankahara, Mike Hilts
*   FIRST VERSION : 	2021-04-05
*   DESCRIPTION   : 	Contains the logic managing the UI for all users
*/

using Lean.Gui;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using static UserInstantiation;

public class UIManagement : MonoBehaviourPunCallbacks
{
    public UserInstantiation userInst;
    public GameObject votingSystem;
    public GameObject stage;
    public GameObject AllUsersMenuResource;
    public GameObject DesktopVotingUIResource;
    public GameObject PerformerPollingUIResource;
    public int perfActorNum;
    private PhotonView view;
    private GameObject PerformerPollingUIResourceButton;
    private Dictionary<string, GameObject> UIContainers = new Dictionary<string, GameObject>();

    /*  Function	:	StartFromScenarioManager()
    *
    *	Description	:	this function sets up the UI for all users and some addional setup for performer
    *
    *	Parameters	:	None
    *
    *	Returns		:	Void
    */
    public void StartFromScenarioManager()
    {
        UIContainers.Add("UserMenu", Instantiate(AllUsersMenuResource));
        UIContainers["UserMenu"].transform.SetParent(gameObject.transform);
        view = gameObject.GetComponent<PhotonView>();
        if ((UserType)PhotonNetwork.LocalPlayer.CustomProperties["Type"] == UserType.Performer)
        {
            UIContainers.Add("PerformerUI", Instantiate(PerformerPollingUIResource));
            UIContainers["PerformerUI"].transform.SetParent(gameObject.transform);
            UIContainers["PerformerUI"].GetComponent<PerformerUserUIController>().stagecontrol = stage;
            PerformerPollingUIResourceButton = UIContainers["PerformerUI"].GetComponent<PerformerUserUIController>().NewPollButton;
            UIContainers["PerformerUI"].GetComponent<PerformerUserUIController>().VotingSystem = this.votingSystem;
            PerformerPollingUIResourceButton.GetComponent<LeanButton>().OnClick.AddListener(VotingShownForDesktopRPC);
            perfActorNum = PhotonNetwork.LocalPlayer.ActorNumber;
            view.RPC("SendActorNum", RpcTarget.OthersBuffered, perfActorNum);
        }
    }

    /*  Function	:	SendActorNum()
    *
    *	Description	:	this function sets the perfActorNum to whatever was passed in
    *
    *	Parameters	:	int _perfActorNum : the number you wish to set as perfActorNum
    *
    *	Returns		:	Void
    */
    [PunRPC]
    public void SendActorNum(int _perfActorNum)
    {
        perfActorNum = _perfActorNum;
    }

    /*  Function	:	VotingShownForDesktopRPC()
    *
    *	Description	:	this function sets up the data to be displayed for desktop users when a Poll is created
    *
    *	Parameters	:	None
    *
    *	Returns		:	Void
    */
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

    /*  Function	:	DisplayVotingPanelRPC()
    *
    *	Description	:	this function gets data to be displayed in the voting UI for desktop users
    *
    *	Parameters	:	string[] theOptions : an array of strings holding the options user can vote for
    *	                string   question   : the question that the users are voting on
    *	                int totalOptions    : the number of options the user can pick from
    *
    *	Returns		:	Void
    */
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

    /*  Function	:	VoteRPC()
    *
    *	Description	:	this function handles the casting of votes by taking in a number and casting a vote to the specified option
    *
    *	Parameters	:	int voteOption : a number representing what option was votes for
    *
    *	Returns		:	Void
    */
    [PunRPC]
    private void VoteRPC(int voteOption)
    {
        if ((UserType)PhotonNetwork.LocalPlayer.CustomProperties["Type"] == UserType.Performer)
        {
            votingSystem.GetComponent<VotingSystemController>().Voting(voteOption);
        }
    }
}
