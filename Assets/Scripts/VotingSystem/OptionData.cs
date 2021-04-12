using UnityEngine;

public class OptionData : MonoBehaviour
{

	public string OptionName;
	public int VoteCount;

	public OptionData()
	{
		VoteCount = 0;
	}

	public OptionData(string optionName)
	{
		this.OptionName = optionName;
		this.VoteCount = 0;
	}
}