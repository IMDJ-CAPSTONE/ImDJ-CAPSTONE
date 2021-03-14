using Lean.Gui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopUserVotingUI : MonoBehaviour
{
    public GameObject[] voteButtons;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < voteButtons.Length; i++)
        {
            voteButtons[i].GetComponent<LeanButton>().OnClick.AddListener(sendVote);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void sendVote()
    {

    }
}
