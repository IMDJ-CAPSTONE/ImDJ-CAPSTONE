/*  FILE          : 	DesltopUserVotingUI.cs
*   PROJECT       : 	PROG3221 - Capstone
*   PROGRAMMER    : 	Ivan Granic, Jason Kassies, Div Dankahara, Mike Hilts
*   FIRST VERSION : 	2021-04-05
*   DESCRIPTION   : 	Contains the logic for desktop user voting
*/

using Lean.Gui;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DesktopUserVotingUI : MonoBehaviour
{
    private GameObject[] voteButtons;
    public GameObject voteButtonPrefab;
    public GameObject votingMenu;
    public GameObject questionText;
    public Action<int> voteOption;

    /*  Function	:	Start()
    *
    *	Description	:	this function get called before anything else happens
    *
    *	Parameters	:	None
    *
    *	Returns		:	Void
    */
    void Start()
    {
        
    }

    /*  Function	:	setupIU()
    *
    *	Description	:	this function sets up the UI for desktop users when a Poll is available
    *
    *	Parameters	:	int optionCount  : the number of options there are in the Poll
    *	                string[] options : an array of strings holding the text of what you are voting for
    *	                string question  : the question asked in the Poll
    *
    *	Returns		:	Void
    */
    public void setupUI(int optionCount, string[] options, string question)
    {
        voteButtons = new GameObject[optionCount];
        questionText.GetComponentInChildren<TMP_Text>().text = question;

        for (int i = 0; i < optionCount; i++)
        {
            int copy = i + 1;
            voteButtons[i] = Instantiate(voteButtonPrefab);
            voteButtons[i].name = "Vote Option: " + i.ToString();
            voteButtons[i].GetComponentInChildren<Text>().text = options[i];
            voteButtons[i].transform.SetParent(votingMenu.transform);
            voteButtons[i].GetComponent<LeanButton>().OnClick.AddListener(() => { sendVote(copy); });
        }
    }

    /*  Function	:	ExitVoteMenu()
    *
    *	Description	:	this function destroys the object containing the voting UI
    *	                its called after a vote is cast
    *
    *	Parameters	:	None
    *
    *	Returns		:	Void
    */
    public void ExitVoteMenu()
    {
        Destroy(gameObject);
    }

    /*  Function	:	sendVote()
    *
    *	Description	:	this function get called when a user selects an option to vote for
    *
    *	Parameters	:	int i : the number representing what the user chose to vote for
    *
    *	Returns		:	Void
    */
    public void sendVote(int i)
    {
        this.voteOption?.Invoke(i);
        Debug.Log(i.ToString());
        ExitVoteMenu();
    }
}
