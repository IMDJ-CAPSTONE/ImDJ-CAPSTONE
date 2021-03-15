using Lean.Gui;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopUserVotingUI : MonoBehaviour
{
    public GameObject[] voteButtons;

    public Action<int> voteOption;
    // Start is called before the first frame update
    void Start()
    {
        voteButtons[0].GetComponent<LeanButton>().OnClick.AddListener(() => { sendVote(1); });
        voteButtons[1].GetComponent<LeanButton>().OnClick.AddListener(() => { sendVote(2); });
        voteButtons[2].GetComponent<LeanButton>().OnClick.AddListener(() => { sendVote(3); });
        voteButtons[3].GetComponent<LeanButton>().OnClick.AddListener(() => { sendVote(4); });
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
