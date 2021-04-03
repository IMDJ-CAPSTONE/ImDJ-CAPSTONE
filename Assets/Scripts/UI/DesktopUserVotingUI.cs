using Lean.Gui;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DesktopUserVotingUI : MonoBehaviour
{
    public GameObject voteButtonPrefab;
    private GameObject[] voteButtons;
    public GameObject votingMenu;
    public GameObject questionText;
    public Action<int> voteOption;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void setupUI(int optionCount, List<string> options, string question)
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

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ExitVoteMenu()
    {
        Destroy(gameObject);
    }
    public void sendVote(int i)
    {
        this.voteOption?.Invoke(i);
        Debug.Log(i.ToString());
        ExitVoteMenu();
    }


}
