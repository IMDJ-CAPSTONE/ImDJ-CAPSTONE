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
            LargeTruss();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LargeTruss()
    {
        topRing = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "StageElements", "TopRing"), transform.position, transform.rotation);
        topRing.transform.localPosition = new Vector3(0f, 7.5f, 0f);
        topRing.transform.parent = LightsGO.transform;
        //anim = topRing.GetComponent<Animator>();
    }
}
