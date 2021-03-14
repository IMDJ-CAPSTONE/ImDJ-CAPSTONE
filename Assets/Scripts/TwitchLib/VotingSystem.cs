using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VotingSystem : MonoBehaviour
{

	public string Question;
	public Dictionary<int, optionData> options = new Dictionary<int, optionData>();
	
	public void AddOption(string optionName) 
	{
		//adding option to options list to keep track of voting count
		
		var option = new optionData(optionName);
		options.Add(options.Count + 1, option);	
	}

	public void NewQuestion(string question) 
	{
		this.Question = question;
    }

	public void Voting(int optionNumber) 
	{
		options[optionNumber].VoteCount += 1;
    }

	public int GetVoteCount(int optionNumber) 
	{ 
		return options[optionNumber].VoteCount;
    }

	public void ClearVoting() {
		options.Clear();
		Question = "";
    }

}
public class optionData {
	
	public string OptionName;
	public int VoteCount;

    public optionData() {
		VoteCount = 0;
    }

    public optionData(string optionName, int voteCount = 0) {
		this.OptionName = optionName;
		this.VoteCount = voteCount;
    }
}