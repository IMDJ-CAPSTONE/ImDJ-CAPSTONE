using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VoiceTest : MonoBehaviour
{
    public GameObject voiceTestPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PhotonNetwork.Instantiate(voiceTestPrefab.name, new Vector3(Random.Range(1f, 5f), Random.Range(1f, 5f), Random.Range(1f, 5f)), voiceTestPrefab.transform.rotation);
    }
}
