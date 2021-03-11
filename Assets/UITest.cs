using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UserInstantiation;

public class UITest : MonoBehaviourPunCallbacks
{
    private PhotonView view;

    public GameObject button;
    public GameObject panel;

    // Start is called before the first frame update
    void Start()
    {
        if ((UserType)PhotonNetwork.LocalPlayer.CustomProperties["Type"] == UserType.Performer)
        {
            panel.SetActive(false);
        }
        else
        {
            panel.SetActive(false);
            button.SetActive(false);
        }

        view = gameObject.GetComponent<PhotonView>();
    }

    private void ButtonClick()
    {
        view.RPC("DisplayPanel", RpcTarget.Others);
    }

    [PunRPC]
    public void DisplayPanel()
    {
        panel.SetActive(true);
    }
}
