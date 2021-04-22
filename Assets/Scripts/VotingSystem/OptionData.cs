/*  FILE          : 	OPtionData.cs
*   PROJECT       : 	PROG3221 - Capstone
*   PROGRAMMER    : 	Ivan Granic, Jason Kassies, Div Dankahara, Mike Hilts
*   FIRST VERSION : 	2021-04-05
*   DESCRIPTION   : 	Contains the class used to store the text and how many votes cast for each option in a Poll
*/
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