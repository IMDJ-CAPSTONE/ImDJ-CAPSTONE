using UnityEngine;

public class OptionData : MonoBehaviour
{

	public string OptionName;
	public int VoteCount;

	public OptionData()
	{
		VoteCount = 0;
	}

	public OptionData(string optionName, int voteCount = 0)
	{
		this.OptionName = optionName;
		this.VoteCount = voteCount;
	}
}