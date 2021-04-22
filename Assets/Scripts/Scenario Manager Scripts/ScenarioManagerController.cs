/*  FILE          : 	ScenarioManagerController.cs
*   PROJECT       : 	PROG3221 - Capstone
*   PROGRAMMER    : 	Ivan Granic, Jason Kassies, Div Dankahara, Mike Hilts
*   FIRST VERSION : 	2021-04-05
*   DESCRIPTION   : 	This class sets up the behind the scenes objects for the performer when they connect
*/

using Photon.Pun;
using System.IO;
using UnityEngine;
using static UserInstantiation;

public class ScenarioManagerController : MonoBehaviour
{
    public GameObject UIManagement;
    public GameObject VotingSystemResource;
    public GameObject StageControllerResource;
    public GameObject VotingSystem;

    /*  Function	:	Start()
    *
    *	Description	:	this function get called before anything else happens
    *
    *	Parameters	:	None
    *
    *	Returns		:	Void
    */
    void Start()
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
