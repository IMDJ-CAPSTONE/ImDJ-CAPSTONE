/*! @file       : 	OPtionData.cs
*   @author     : 	Ivan Granic, Jason Kassies, Div Dankahara, Mike Hilts
*   @date       : 	2021-03-01
*   @brief      : 	Contains the class used to store the text and how many votes cast for each option in a Poll
*/

using UnityEngine;

/*! <summary>
*  Contains the class used to store the text and how many votes cast for each option in a Poll
*  </summary>
*/
public class OptionData : MonoBehaviour
{
    public string OptionName;
    public int VoteCount;

    /*! <summary>
     *  default constructor of the class, sets VoteCount to zero
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    public OptionData()
    {
        VoteCount = 0;
    }

    /*! <summary>
     *  Overloaded constructor of the class, takes in OptionName
     *  </summary>
     *  <param name="optionName">a string setting the name of the option</param>
     *  <returns>void</returns>
     */
    public OptionData(string optionName)
    {
        this.OptionName = optionName;
        this.VoteCount = 0;
    }
}
