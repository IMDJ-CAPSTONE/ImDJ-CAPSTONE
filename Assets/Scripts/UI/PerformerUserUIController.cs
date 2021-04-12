using Lean.Gui;
using TMPro;
using UnityEngine;

public class PerformerUserUIController : MonoBehaviour
{
    public GameObject[] OptionSets;
    public GameObject[] DisplayOptionSets;
    public GameObject PollQuestion;
    public GameObject VotingSystem;
    public GameObject NewPollButton;
    public GameObject optionPrefab;
    public GameObject displayOptionPrefab;
    public GameObject twitchDashboard;
    public GameObject mainDashboard;
    public GameObject AddOptions;
    
    // Start is called before the first frame update
    void Start()
    {
        //setup twitchDashboard
        OptionSets = new GameObject[4];

        for (int i = 0; i < 4; i++)
        {
            int copy = i+1;
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
        DisplayOptionSets[2].SetActive(false);
        DisplayOptionSets[3].SetActive(false);
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
        Debug.Log("CreateNewPoll()");
        //first clear out old options
        VotingSystem.GetComponent<VotingSystemController>().ClearVoting();
        
        //check question length
        string ques = PollQuestion.GetComponent<TMP_InputField>().text;
        if (ques.Length > 70)
        {
            ques = Truncate(PollQuestion.GetComponent<TMP_InputField>().text, 70);
        }
        //pass the question
        VotingSystem.GetComponent<VotingSystemController>().NewQuestion(ques);

        foreach (GameObject gO in OptionSets)
        {
            //check if the text box is filled in
            if(gO.GetComponentInChildren<TMP_InputField>().text != "")
            {
                string tmp = gO.GetComponentInChildren<TMP_InputField>().text;
                if(tmp.Length > 70)
                {
                    tmp = Truncate(tmp, 70);
                }
                VotingSystem.GetComponent<VotingSystemController>().AddOption(tmp);
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

    public static string Truncate(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }








}
