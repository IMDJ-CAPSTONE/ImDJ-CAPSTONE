/*! @file       : 	DesltopUserVotingUI.cs
*   @author     : 	Ivan Granic, Jason Kassies, Div Dankahara, Mike Hilts
*   @date       : 	2021-03-12
*   @brief      : 	Contains the logic for desktop user voting
*/

using Lean.Gui;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*! <summary>
*  Contains the logic for desktop user voting
*  </summary>
*/
public class DesktopUserVotingUI : MonoBehaviour
{
    private GameObject[] voteButtons;
    public GameObject voteButtonPrefab;
    public GameObject votingMenu;
    public GameObject questionText;
    public Action<int> voteOption;

    /*! <summary>
     *  this function get called before anything else happens
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    /*! <summary>
     *  this function sets up the UI for desktop users when a Poll is available
     *  </summary>
     *  <param name="optionCount">the number of options there are in the Poll</param>
     *  <param name="options">an array of strings holding the text of what you are voting for</param>
     *  <param name="question">the question asked in the Poll</param>
     *  <returns>void</returns>
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

    /*! <summary>
     *  this function destroys the object containing the voting UI its called after a vote is cast
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    public void ExitVoteMenu()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Destroy(gameObject);
    }

    /*! <summary>
     *  this function get called when a user selects an option to vote for
     *  </summary>
     *  <param name="vote">the number representing what the user chose to vote for</param>
     *  <returns>void</returns>
     */
    public void sendVote(int vote)
    {
        this.voteOption?.Invoke(vote);
        Debug.Log(vote.ToString());
        ExitVoteMenu();
    }
}
