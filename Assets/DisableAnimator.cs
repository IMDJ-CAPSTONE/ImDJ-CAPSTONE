using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UserInstantiation;

public class DisableAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if((UserType)PhotonNetwork.LocalPlayer.CustomProperties["Type"] != UserType.Performer)
        {
            Destroy(this.GetComponent<Animator>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
