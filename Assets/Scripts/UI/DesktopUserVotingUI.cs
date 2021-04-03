using Lean.Gui;
using System;
using UnityEngine;

public class DesktopUserVotingUI : MonoBehaviour
{
    public GameObject voteButtonPrefab;
    private GameObject[] voteButtons;
    public GameObject votingMenu;

    public Action<int> voteOption;

    public int optionCount;

    // Start is called before the first frame update
    void Start()
    {
        //

        voteButtons = new GameObject[optionCount];

        for (int i = 0; i < optionCount; i++)
        {
            int copy = i + 1;
            voteButtons[i] = Instantiate(voteButtonPrefab);
            voteButtons[i].name = "Vote Option: " + i.ToString(); 
            voteButtons[i].transform.SetParent(votingMenu.transform);//need to change parent
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
