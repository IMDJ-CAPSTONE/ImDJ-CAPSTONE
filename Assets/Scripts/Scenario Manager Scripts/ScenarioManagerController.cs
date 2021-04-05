using Photon.Pun;
using UnityEngine;
using static UserInstantiation;
using Photon.Pun;
using System.IO;

public class ScenarioManagerController : MonoBehaviour
{
    public GameObject UIManagement;
    public GameObject VotingSystemResource;
    public GameObject StageControllerResource;
    public GameObject VotingSystem;

    // Start is called before the first frame update
    void Start()
    {
        if ((UserType)PhotonNetwork.LocalPlayer.CustomProperties["Type"] == UserType.Performer)
        {
            VotingSystem = Instantiate(VotingSystemResource);
            VotingSystem.transform.SetParent(gameObject.transform);
            UIManagement.GetComponent<UIManagement>().votingSystem = this.VotingSystem;

            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Stage Components"), transform.position, transform.rotation);
        }
        UIManagement.GetComponent<UIManagement>().StartFromScenarioManager();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
