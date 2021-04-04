using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StageTopLevelController : MonoBehaviour
{

    private PhotonView PV;
    public GameObject LightsGO;
    public GameObject topRing;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            TopRing();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TopRing()
    {
        topRing = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "StageElements", "TopRing"), transform.position, transform.rotation);
        PV.RPC("setLightParentAndPos", RpcTarget.AllBuffered);
        //anim = topRing.GetComponent<Animator>();
    }


    [PunRPC]
    private void setLightParentAndPos()
    {
        topRing = GameObject.FindGameObjectWithTag("StageLights");
        print("rpc called");
        topRing.transform.parent = LightsGO.transform;
        topRing.transform.localPosition = new Vector3(0f, 7.5f, 0f);
    }
}
