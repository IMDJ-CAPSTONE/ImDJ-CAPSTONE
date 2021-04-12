using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StageTopLevelController : MonoBehaviour
{

    private PhotonView PV;
    public GameObject LightsGO;
    public GameObject Lights;
    public Animator anim;

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
            TopRing();
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
            }
        }
    }

    private void TopRing()
    {
        Lights = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "StageElements", "TopRing"), transform.position, transform.rotation);
        PV.RPC("setLightParentAndPos", RpcTarget.AllBuffered);
        //anim = topRing.GetComponent<Animator>();
    }


    [PunRPC]
    private void setLightParentAndPos()
    {
        Lights = GameObject.FindGameObjectWithTag("StageLights");
        Lights.transform.parent = LightsGO.transform;
        //Lights.transform.localPosition = new Vector3(0f, 7.5f, 0f);
    }
}
