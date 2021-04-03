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
    
    // Start is called before the first frame update
    void Start()
    {
        OptionSets = new GameObject[4];
        
        //these 2 line are supposed to to move the button to the bottom of the screen
        ///RectTransform rectTransform = NewPollButton.GetComponent<RectTransform>();
        ///rectTransform.anchoredPosition = new Vector2(0,400);


        for (int i = 0; i < 4; i++)
        {
            int copy = i+1;
            OptionSets[i] = Instantiate(optionPrefab);
            OptionSets[i].name = "Option: " + copy.ToString();
            OptionSets[i].transform.SetParent(twitchDashboard.transform); //need to change parent
            OptionSets[i].transform.SetSiblingIndex(copy);
            OptionSets[i].GetComponentInChildren<TMP_Text>().text = "Option: " + copy.ToString();
            //OptionSets[i].GetComponent<TMP_Text>().text = "Option "+ copy;    //this should set the lable to increment but instead it crashes
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

        //reset option text boxes
        foreach (GameObject gO in OptionSets)
        {
            gO.GetComponentInChildren<TMP_InputField>().text = "";
        }
    }

}
