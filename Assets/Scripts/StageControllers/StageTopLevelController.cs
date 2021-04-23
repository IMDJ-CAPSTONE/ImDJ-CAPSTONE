/*! @file       : 	StageTopLevelController.cs
*   @author     : 	Ivan Granic, Jason Kassies, Div Dankahara, Mike Hilts
*   @date       : 	2021-03-01
*   @brief      : 	Contains the logic for altering the stage visuals in Unity
*/

using Photon.Pun;
using System.IO;
using UnityEngine;

/*! <summary>
*  Contains the logic for altering the stage visuals in Unity
*  </summary>
*/
public class StageTopLevelController : MonoBehaviour
{
    private AbletonLink link;
    private PhotonView PV;
    public GameObject LightsGO;
    public GameObject Lights;
    public GameObject BackgroundGO;
    public GameObject Background;

    private long lastbeatnum;
    private long beatnum;

    /*! <summary>
     *  This function get executed before anything else in this file, it sets up values for the photon view
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    private void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            lastbeatnum = 0;
            beatnum = 0;
        }
    }

    /*! <summary>
     *  this function is called once per frame, it sends the bpm of the music to the
     *  visuals so they move with the beat of the song
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    private void Update()
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

    /*! <summary>
     *  this function deletes the object StageLights and then reinstantiates a differnet set of lights
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    public void LITE1()
    {
        if (GameObject.FindGameObjectWithTag("StageLights"))
        {
            PhotonNetwork.Destroy(GameObject.FindGameObjectWithTag("StageLights"));
        }
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "StageElements", "LITE1"), transform.position, transform.rotation);
        PV.RPC("setLightParentAndPos", RpcTarget.AllBuffered);
    }

    /*! <summary>
     *  this function deletes the object StageLights and then reinstantiates a differnet set of lights
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
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

    /*! <summary>
     *  this function gets a gameObject with the tag StageLights and sets it transform
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    [PunRPC]
    private void setLightParentAndPos()
    {
        Lights = GameObject.FindGameObjectWithTag("StageLights");
        Lights.transform.parent = LightsGO.transform;
    }

    #endregion Lights

    #region Backgrounds

    /*! <summary>
     *  this function deletes the object BackGround and then reinstantiates a differnet background object
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
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

    /*! <summary>
     *  this function deletes the object BackGround and then reinstantiates a differnet background object
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
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

    /*! <summary>
     *  this function gets a gameObject with the tag BackGround and sets it transform
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    [PunRPC]
    private void setBackgroundParentAndPos()
    {
        Background = GameObject.FindGameObjectWithTag("BackGround");
        Background.transform.parent = BackgroundGO.transform;
    }

    #endregion Backgrounds
}
