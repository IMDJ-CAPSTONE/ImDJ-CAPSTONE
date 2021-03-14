using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UserInstantiation;

public class UIManagement : MonoBehaviourPunCallbacks
{
    private PhotonView view;

    public GameObject DesktopUIResource;
    
    public GameObject PerformerUIResource;

    private Dictionary<string, GameObject> UIContainers = new Dictionary<string, GameObject>();


    public GameObject button;
    public GameObject twitchPanel;

    // Start is called before the first frame update
    void Start()
    {
        if ((UserType)PhotonNetwork.LocalPlayer.CustomProperties["Type"] == UserType.Performer)
        {
            UIContainers.Add("PerformerUI", Instantiate(PerformerUIResource));
            UIContainers["PerformerUI"].transform.SetParent(gameObject.transform);
            button = UIContainers["PerformerUI"].GetComponent<PerformerUserUIController>().button;
        }
        else if ((UserType)PhotonNetwork.LocalPlayer.CustomProperties["Type"] == UserType.Desktop)
        {
            UIContainers.Add("DesktopUI", Instantiate(DesktopUIResource));
            UIContainers["DesktopUI"].transform.SetParent(gameObject.transform);
            twitchPanel = UIContainers["DesktopUI"];
            twitchPanel.SetActive(false);
        }

        view = gameObject.GetComponent<PhotonView>();
    }

    public void ButtonClick()
    {
        view.RPC("DisplayPanel", RpcTarget.Others);
    }

    [PunRPC]
    private void DisplayPanel()
    {
        twitchPanel.SetActive(true);
    }
}
