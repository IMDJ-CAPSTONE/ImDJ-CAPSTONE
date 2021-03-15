using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PerformerUserUIController : MonoBehaviour
{

    public GameObject[] OptionSets;
    public GameObject PollQuestion;
    public GameObject VotingSystem;

    public GameObject NewPollButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateNewPoll()
    {
        Debug.Log("youre pressed eh?");
        VotingSystem.GetComponent<VotingSystemController>().NewQuestion(PollQuestion.GetComponent<TMP_InputField>().text);
        foreach (GameObject gO in OptionSets)
        {
            VotingSystem.GetComponent<VotingSystemController>().AddOption(gO.GetComponent<TMP_InputField>().text);
        }
        VotingSystem.GetComponent<VotingSystemController>().SendPollToChat();
    }

}
