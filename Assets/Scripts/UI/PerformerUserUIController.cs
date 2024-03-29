﻿/*! @file       : 	PerformerUserUIController.cs
*   @author     : 	Ivan Granic, Jason Kassies, Div Dankahara, Mike Hilts
*   @date       : 	2021-03-01
*   @brief      : 	Contains the logic for the UI of the Performer, the code for all the buttons they can press
*/

using Lean.Gui;
using TMPro;
using UnityEngine;

/*! <summary>
*  Contains the logic for the UI of the Performer, the code for all the buttons they can press
*  </summary>
*/
public class PerformerUserUIController : MonoBehaviour
{
    #region publicGameObjects

    public GameObject[] OptionSets;
    public GameObject[] DisplayOptionSets;
    public GameObject PollQuestion;
    public GameObject DisplayQuestion;
    public GameObject VotingSystem;
    public GameObject NewPollButton;
    public GameObject optionPrefab;
    public GameObject displayOptionPrefab;
    public GameObject twitchDashboard;
    public GameObject mainDashboard;
    public GameObject AddOptions;
    public GameObject stagecontrol;

    #endregion publicGameObjects

    /*! <summary>
     *  This function get executed before anything else in this file
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    private void Start()
    {
        //setup twitchDashboard
        OptionSets = new GameObject[4];

        for (int i = 0; i < 4; i++)
        {
            int copy = i + 1;
            OptionSets[i] = Instantiate(optionPrefab);
            OptionSets[i].name = "Option: " + copy.ToString();
            OptionSets[i].transform.SetParent(twitchDashboard.transform);
            OptionSets[i].transform.SetSiblingIndex(copy);
            OptionSets[i].GetComponentInChildren<TMP_Text>().text = "Option: " + copy.ToString();
        }
        OptionSets[2].SetActive(false);
        OptionSets[3].SetActive(false);
        twitchDashboard.GetComponent<LeanToggle>().TurnOff();

        //setup mainDashboard
        DisplayOptionSets = new GameObject[4];
        
        for (int i = 0; i < 4; i++)
        {
            int copy = i + 1;
            DisplayOptionSets[i] = Instantiate(displayOptionPrefab);
            DisplayOptionSets[i].name = "Option: " + copy.ToString();
            DisplayOptionSets[i].transform.SetParent(mainDashboard.transform);
            DisplayOptionSets[i].transform.SetSiblingIndex(copy);
            DisplayOptionSets[i].GetComponentInChildren<TMP_Text>().text = "Option: " + copy.ToString();
        }
        DisplayOptionSets[0].SetActive(false);
        DisplayOptionSets[1].SetActive(false);
        DisplayOptionSets[2].SetActive(false);
        DisplayOptionSets[3].SetActive(false);
    }

    /*! <summary>
	 *  This function gets called when the performer clicks the "Add Option" button on Twitch Dashboard
	 *  it shows another text box to add addional option for a Poll, up to a max of 4.
	 *  </summary>
	 *  <param name="none"></param>
	 *  <returns>void</returns>
	 */
    public void AddOption()
    {
        if (OptionSets[2].activeSelf == false)
        {
            OptionSets[2].SetActive(true);
        }
        else if (OptionSets[3].activeSelf == false)
        {
            OptionSets[3].SetActive(true);
            AddOptions.SetActive(false);
        }
    }

    /*! <summary>
	 *  This function gets called when the performer clicks the "Create Poll" button on Twitch Dashboard
	 *  it clears the info from the last poll, truncates the incoming data for the new poll and initalizes the new poll
	 *  it then resets the textboxes, and updates the Main Dashboard to show the results
	 *  </summary>
	 *  <param name="none"></param>
	 *  <returns>void</returns>
	 */
    public void CreateNewPoll()
    {
        Debug.Log("CreateNewPoll()");
        //first clear out old options
        VotingSystem.GetComponent<VotingSystemController>().ClearVoting();

        //check question length
        string ques = PollQuestion.GetComponent<TMP_InputField>().text;
        if (ques.Length > 70)
        {
            ques = Truncate(ques, 70);
        }
        //pass the question
        VotingSystem.GetComponent<VotingSystemController>().NewQuestion(ques);

        foreach (GameObject gO in OptionSets)
        {
            //check if the text box is filled in
            if (gO.GetComponentInChildren<TMP_InputField>().text != "")
            {
                string optionText = gO.GetComponentInChildren<TMP_InputField>().text;
                if (optionText.Length > 70)
                {
                    optionText = Truncate(optionText, 70);
                }
                //add the option
                VotingSystem.GetComponent<VotingSystemController>().AddOption(optionText);
            }
        }
        VotingSystem.GetComponent<VotingSystemController>().SendPollToChat();
        VotingSystem.GetComponent<VotingSystemController>().setActive(true);

        //reset Poll textboxes
        OptionSets[0].GetComponentInChildren<TMP_InputField>().text = "";
        OptionSets[1].GetComponentInChildren<TMP_InputField>().text = "";
        OptionSets[2].GetComponentInChildren<TMP_InputField>().text = "";
        OptionSets[3].GetComponentInChildren<TMP_InputField>().text = "";
        OptionSets[2].SetActive(false);
        OptionSets[3].SetActive(false);
        AddOptions.SetActive(true);

        //setup main dashboard
        updateMainDash();
        InvokeRepeating("updateMainDash", 0.1f, 3f);
    }

    /*! <summary>
	 *  This function gets called when the performer clicks the "Stop Voting" button on Main Dashboard
	 *  It sets the voting system to: stop counting new votes, send a message to twitch chat that the poll has ended
	 *  with the results from the poll and updates the Main Dashboard with the final tally
	 *  </summary>
	 *  <param name="none"></param>
	 *  <returns>void</returns>
	 */
    public void finalizePoll()
    {
        VotingSystem.GetComponent<VotingSystemController>().setActive(false);
        VotingSystem.GetComponent<VotingSystemController>().endPoll();
        VotingSystem.GetComponent<VotingSystemController>().cancelInvoke();
        VotingSystem.GetComponent<VotingSystemController>().SentResultToChat();
        CancelInvoke("updateMainDash");
        updateMainDash();
    }

    /*! <summary>
	 *  This function updates the text on the Main Dahsboard indicating the most up-to-date data on the Poll
	 *  </summary>
	 *  <param name="none"></param>
	 *  <returns>void</returns>
	 */
    public void updateMainDash()
    {
        DisplayQuestion.GetComponentInChildren<TMP_Text>().text = VotingSystem.GetComponent<VotingSystemController>().Question;

        for (int i = 0; i < DisplayOptionSets.Length; i++)
        {
            DisplayOptionSets[i].SetActive(true);
            string ques = VotingSystem.GetComponent<VotingSystemController>().GetOptionText(i + 1);
            int votes = VotingSystem.GetComponent<VotingSystemController>().GetVoteCount(i + 1);
            if (ques != null)
            {
                DisplayOptionSets[i].GetComponentInChildren<TMP_Text>().text =
                "Option " + (i + 1).ToString() + ":  \"" + ques + "\" \tTotal votes: " + votes.ToString();
            }
            else
            {
                DisplayOptionSets[i].SetActive(false);
            }
        }
    }

    /*! <summary>
    *  this function takes in a string and an int and truncates the string to be of that length
    *  </summary>
    *  <param name="value">The string that needs to be checked/truncated</param>
    *  <param name="maxLength">The length the string needs to be less than</param>
    *  <returns>string value : The resulting string after truncation</returns>
    */
    public static string Truncate(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }

    /*! <summary>
     *  This function gets called when the performer clicks the "Light Set 1" button on Visuals Dashboard
     *  It sets the lights that are being displayed to set 1, regardless of what is currently on
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    public void lightSet1()
    {
        stagecontrol.GetComponent<StageTopLevelController>().LITE1();
    }

    /*! <summary>
     *  This function gets called when the performer clicks the "Light Set 2" button on Visuals Dashboard
     *  It sets the lights that are being displayed to set 2, regardless of what is currently on
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    public void lightSet2()
    {
        stagecontrol.GetComponent<StageTopLevelController>().LITE2();
    }

    /*! <summary>
     *  This function gets called when the performer clicks the "Background Set 1" button on Visuals Dashboard
     *  It sets the background object that are being displayed to set 1, regardless of what is currently on
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    public void backgroundSet1()
    {
        stagecontrol.GetComponent<StageTopLevelController>().BG1();
    }

    /*! <summary>
     *  This function gets called when the performer clicks the "Background Set 1" button on Visuals Dashboard
     *  It sets the background object that are being displayed to set 2, regardless of what is currently on
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    public void backgroundSet2()
    {
        stagecontrol.GetComponent<StageTopLevelController>().BG2();
    }
}
