using Photon.Pun;
using System.IO;
using UnityEngine;

public class StageTopLevelController : MonoBehaviour
{

    private PhotonView PV;
    public GameObject LightsGO;
    public GameObject Lights;
    public GameObject BackgroundGO;
    public GameObject Background;

    long lastbeatnum;
    long beatnum;

    private AbletonLink link;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            lastbeatnum = 0;
            beatnum = 0;
        }
    }

    // Update is called once per frame
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
    public void LITE1()
    {
        if(GameObject.FindGameObjectWithTag("StageLights"))
        {
            PhotonNetwork.Destroy(GameObject.FindGameObjectWithTag("StageLights"));
        }
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "StageElements", "LITE1"), transform.position, transform.rotation);
        PV.RPC("setLightParentAndPos", RpcTarget.AllBuffered);
    }
    public void LITE2()
    {
        if (GameObject.FindGameObjectWithTag("StageLights"))
        {
            PhotonNetwork.Destroy(GameObject.FindGameObjectWithTag("StageLights"));
        }
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "StageElements", "LITE2"), transform.position, transform.rotation);
        PV.RPC("setLightParentAndPos", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void setLightParentAndPos()
    {
        Lights = GameObject.FindGameObjectWithTag("StageLights");
        Lights.transform.parent = LightsGO.transform;
    }
    #endregion

    #region Backgrounds
    public void BG1()
    {
        if (GameObject.FindGameObjectWithTag("BackGround"))
        {
            PhotonNetwork.Destroy(GameObject.FindGameObjectWithTag("BackGround"));
        }
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "StageElements", "BG1"), transform.position, transform.rotation);
        PV.RPC("setBackgroundParentAndPos", RpcTarget.AllBuffered);
    }
    public void BG2()
    {
        if (GameObject.FindGameObjectWithTag("BackGround"))
        {
            PhotonNetwork.Destroy(GameObject.FindGameObjectWithTag("BackGround"));
        }
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "StageElements", "BG2"), transform.position, transform.rotation);
        PV.RPC("setBackgroundParentAndPos", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void setBackgroundParentAndPos()
    {
        Background = GameObject.FindGameObjectWithTag("BackGround");
        Background.transform.parent = BackgroundGO.transform;
    }

    #endregion

}
