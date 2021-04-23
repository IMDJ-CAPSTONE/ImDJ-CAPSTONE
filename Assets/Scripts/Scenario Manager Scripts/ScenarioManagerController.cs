/*! @file       : 	ScenarioManagerController.cs
*   @author     : 	Ivan Granic, Jason Kassies, Div Dankahara, Mike Hilts
*   @date       : 	2021-02-010
*   @brief      : 	This class sets up the the Twitch bot and voting system for the performer when they connect
*/

using Photon.Pun;
using System.IO;
using UnityEngine;
using static UserInstantiation;

/*! <summary>
*  This class sets up the the Twitch bot and voting system for the performer when they connect
*  </summary>
*/
public class ScenarioManagerController : MonoBehaviour
{
    public GameObject UIManagement;
    public GameObject VotingSystemResource;
    public GameObject StageControllerResource;
    public GameObject VotingSystem;

    /*! <summary>
     *  This function get executed before anything else in this file, it sets up the Twitch bot and voting system
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    private void Start()
    {
        if ((UserType)PhotonNetwork.LocalPlayer.CustomProperties["Type"] == UserType.Performer)
        {
            VotingSystem = Instantiate(VotingSystemResource);
            VotingSystem.transform.SetParent(gameObject.transform);
            UIManagement.GetComponent<UIManagement>().votingSystem = this.VotingSystem;
            UIManagement.GetComponent<UIManagement>().stage =
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Stage Components"), transform.position, transform.rotation);
            Debug.Log("");
        }
        UIManagement.GetComponent<UIManagement>().StartFromScenarioManager();
    }
}
