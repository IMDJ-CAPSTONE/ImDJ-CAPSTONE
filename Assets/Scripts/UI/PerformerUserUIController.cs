using TMPro;
using UnityEngine;

public class PerformerUserUIController : MonoBehaviour
{
    public GameObject[] OptionSets;
    public GameObject PollQuestion;
    public GameObject VotingSystem;
    public GameObject NewPollButton;
    public GameObject optionPrefab;
    public GameObject twitchDashboard;
    public GameObject AddOptions;
    
    // Start is called before the first frame update
    void Start()
    {
        OptionSets = new GameObject[4];

        for (int i = 0; i < 4; i++)
        {
            int copy = i+1;
            OptionSets[i] = Instantiate(optionPrefab);
            OptionSets[i].name = "Option: " + copy.ToString();
            OptionSets[i].transform.SetParent(twitchDashboard.transform); //need to change parent
            OptionSets[i].transform.SetSiblingIndex(copy);
            OptionSets[i].GetComponentInChildren<TMP_Text>().text = "Option: " + copy.ToString();
        }
        OptionSets[2].SetActive(false);
        OptionSets[3].SetActive(false);
    }

    public void AddOption()
    {
        if(OptionSets[2].activeSelf == false)
        {
            OptionSets[2].SetActive(true);
        }
        else if(OptionSets[3].activeSelf == false)
        {
            OptionSets[3].SetActive(true);
            AddOptions.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateNewPoll()
    {
        //first clear out old options
        VotingSystem.GetComponent<VotingSystemController>().ClearVoting();

        Debug.Log("CreateNewPoll()");
        VotingSystem.GetComponent<VotingSystemController>().NewQuestion(PollQuestion.GetComponent<TMP_InputField>().text);
        foreach (GameObject gO in OptionSets)
        {
            ///VotingSystem.GetComponent<VotingSystemController>().AddOption(gO.GetComponent<TMP_InputField>().text);
            //check if the text box is filled in
            if(gO.GetComponentInChildren<TMP_InputField>().text != "")
            {
                VotingSystem.GetComponent<VotingSystemController>().AddOption(gO.GetComponentInChildren<TMP_InputField>().text);
            }
            
        }
        VotingSystem.GetComponent<VotingSystemController>().SendPollToChat();

        //reset Poll textboxes
        OptionSets[0].GetComponentInChildren<TMP_InputField>().text = "";
        OptionSets[1].GetComponentInChildren<TMP_InputField>().text = "";
        OptionSets[2].GetComponentInChildren<TMP_InputField>().text = "";
        OptionSets[2].SetActive(false);
        OptionSets[3].GetComponentInChildren<TMP_InputField>().text = "";
        OptionSets[3].SetActive(false);
        AddOptions.SetActive(true);

    }

}
