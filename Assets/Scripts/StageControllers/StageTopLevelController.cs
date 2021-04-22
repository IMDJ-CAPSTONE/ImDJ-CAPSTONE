/*  FILE          : 	StageTopLevelController.cs
*   PROJECT       : 	PROG3221 - Capstone
*   PROGRAMMER    : 	Ivan Granic, Jason Kassies, Div Dankahara, Mike Hilts
*   FIRST VERSION : 	2021-04-05
*   DESCRIPTION   : 	Contains the logic for altering the visuals in Unity
*/

using Photon.Pun;
using System.IO;
using UnityEngine;

public class StageTopLevelController : MonoBehaviour
{
    private AbletonLink link;
    private PhotonView PV;
    public GameObject LightsGO;
    public GameObject Lights;
    public GameObject BackgroundGO;
    public GameObject Background;

    long lastbeatnum;
    long beatnum;

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
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            lastbeatnum = 0;
            beatnum = 0;
        }
    }

    /*  Function	:	Update()
    *
    *	Description	:	this function is classed once per frame
    *
    *	Parameters	:	None
    *
    *	Returns		:	Void
    */
    void Update()
    {
        if (PV.IsMine)
        {
            lastbeatnum = beatnum;
            double beat, phase, tempo, time;
            int numPeers;
            AbletonLink.Instance.update(out beat, out phase, out tempo, out time, out numPeers);
            beatnum = (long)beat;

            // We can obtain the latest beat and phase like this.
            Debug.Log("beat: " + beatnum + " phase:" + phase + " numpeers:" + numPeers + " tempo:" + tempo);

            float tempoF = (float)tempo;
            if ((beatnum - lastbeatnum) == 1)
            {
                if (Lights != null)
                {
                    Lights.GetComponent<ElementBeatController>().setBeat(tempoF);
                }
                if (Background != null)
                {
                    Background.GetComponent<ElementBeatController>().setBeat(tempoF);
                }
            }
        }
    }

    #region Lights

    /*  Function	:	LITE1()
    *
    *	Description	:	this function deletes the object StageLights and then reinstantiates a differnet set of lights
    *
    *	Parameters	:	None
    *
    *	Returns		:	Void
    */
    public void LITE1()
    {
        if(GameObject.FindGameObjectWithTag("StageLights"))
        {
            PhotonNetwork.Destroy(GameObject.FindGameObjectWithTag("StageLights"));
        }
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "StageElements", "LITE1"), transform.position, transform.rotation);
        PV.RPC("setLightParentAndPos", RpcTarget.AllBuffered);
    }

    /*  Function	:	LITE2()
    *
    *	Description	:	this function deletes the object StageLights and then reinstantiates a differnet set of lights
    *
    *	Parameters	:	None
    *
    *	Returns		:	Void
    */
    public void LITE2()
    {
        if (GameObject.FindGameObjectWithTag("StageLights"))
        {
            PhotonNetwork.Destroy(GameObject.FindGameObjectWithTag("StageLights"));
        }
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "StageElements", "LITE2"), transform.position, transform.rotation);
        PV.RPC("setLightParentAndPos", RpcTarget.AllBuffered);
    }

    /*  Function	:	setLightParentAndPos()
    *
    *	Description	:	this function gets a gameObject with the tag StageLights and sets it transform
    *
    *	Parameters	:	None
    *
    *	Returns		:	Void
    */
    [PunRPC]
    private void setLightParentAndPos()
    {
        Lights = GameObject.FindGameObjectWithTag("StageLights");
        Lights.transform.parent = LightsGO.transform;
    }
    #endregion

    #region Backgrounds

    /*  Function	:	BG1()
    *
    *	Description	:	this function deletes the object BackGround and then reinstantiates a differnet background object
    *
    *	Parameters	:	None
    *
    *	Returns		:	Void
    */
    public void BG1()
    {
        if (GameObject.FindGameObjectWithTag("BackGround"))
        {
            PhotonNetwork.Destroy(GameObject.FindGameObjectWithTag("BackGround"));
        }
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "StageElements", "BG1"), transform.position, transform.rotation);
        PV.RPC("setBackgroundParentAndPos", RpcTarget.AllBuffered);
    }

    /*  Function	:	BG2()
    *
    *	Description	:	this function deletes the object BackGround and then reinstantiates a differnet background object
    *
    *	Parameters	:	None
    *
    *	Returns		:	Void
    */
    public void BG2()
    {
        if (GameObject.FindGameObjectWithTag("BackGround"))
        {
            PhotonNetwork.Destroy(GameObject.FindGameObjectWithTag("BackGround"));
        }
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "StageElements", "BG2"), transform.position, transform.rotation);
        PV.RPC("setBackgroundParentAndPos", RpcTarget.AllBuffered);
    }

    /*  Function	:	setBackgroundParentAndPos()
    *
    *	Description	:	this function gets a gameObject with the tag BackGround and sets it transform
    *
    *	Parameters	:	None
    *
    *	Returns		:	Void
    */
    [PunRPC]
    private void setBackgroundParentAndPos()
    {
        Background = GameObject.FindGameObjectWithTag("BackGround");
        Background.transform.parent = BackgroundGO.transform;
    }

    #endregion

}
