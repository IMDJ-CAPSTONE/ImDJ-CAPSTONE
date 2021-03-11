/*
 *  FILE          :	UIController.cs
 *  PROJECT       :	ImDJ Capstone Project 
 *  PROGRAMMER    :	Michael Hilts - 5377643
 *  FIRST VERSION :	Feb 6, 2021
 *  DESCRIPTION   : This file contains the UIController class which contains all the on click
 *                  methods for the UI buttons.
 */

#region Resources

using ExitGames.Client.Photon;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

#endregion

[RequireComponent(typeof(AudioSource))]
public class UIController : MonoBehaviour
{
    #region fields

    [Tooltip("The UI to use if the player is not using VR.")]
    [SerializeField]
    private GameObject DesktopMenu;

    [Tooltip("The UI to use if the player is using VR.")]
    [SerializeField]
    private GameObject VRMenu;

    private AudioSource clickAudio;
    private TextMeshProUGUI debugText;  // the debug text to print messages to the screen
    private bool usingVR = false;

    #endregion

    #region Properties

    // allows printing of messages to the onscreen debug text
    public string DebugText
    {
        set
        {
            debugText.text = value;    
        }
    }

    #endregion

    #region MonoBehaviour Callbacks

    /*
     * METHOD     : Start()
     * DESCRIPTION: Called before the first frame update. This will get a reference to the
     *              debug text.
     * PARAMETERS : 
     *      VOID
     * RETURNS    : 
     *      VOID
     */

    private void Start()
    {
        clickAudio = GetComponent<AudioSource>();
        //usingVR = OpenVR.IsHmdPresent();
        //DesktopMenu.SetActive(!usingVR);
        //VRMenu.SetActive(usingVR);
        VRMenu.SetActive(false);

        debugText = GameObject.FindGameObjectWithTag("Debug").GetComponent<TextMeshProUGUI>();
    }

    #endregion

    #region Public Methods

    /*
     * METHOD     : ClickAudioPlay()
     * DESCRIPTION: Public method to allow other classes to play the audio. 
     * PARAMETERS : 
     *      VOID
     * RETURNS    : 
     *      VOID
     */

    public void PlayClickAudio()
    {
        clickAudio.Play();
    }

    /*
     * METHOD     : Quit()
     * DESCRIPTION: Public method to allow other classes to exit the application.
     * PARAMETERS : 
     *      VOID
     * RETURNS    : 
     *      VOID
     */

    public void Quit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                    Application.Quit(0);
        #endif
    }

    #endregion

    #region Button Click Handlers

    /*
     * METHOD     : PeformerUserStart()
     * DESCRIPTION: Called when the PeformerUserStart buttton is clicked. Will run the photon test
     *              to attempt to connect to the photon servers.
     * PARAMETERS : 
     *      VOID
     * RETURNS    : 
     *      VOID
     */

    public void PeformerUserStart()
    {
        PlayClickAudio();
        PhotonController.Instance.Connect();
        Hashtable hash = new Hashtable();
        hash.Add("Type", UserInstantiation.UserType.Performer);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    /*
     * METHOD     : PeformerUserStart()
     * DESCRIPTION: Called when the PeformerUserStart buttton is clicked. Will run the photon test
     *              to attempt to connect to the photon servers.
     * PARAMETERS : 
     *      VOID
     * RETURNS    : 
     *      VOID
     */

    public void DesktopUserStart()
    {
        PlayClickAudio();
        PhotonController.Instance.Connect();
        Hashtable hash = new Hashtable();
        hash.Add("Type", UserInstantiation.UserType.Desktop);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    /*
     * METHOD     : VRTestClick()
     * DESCRIPTION: Called when the VR test button is clicked. Will take the user to the
     *              VR test scene.
     * PARAMETERS : 
     *      VOID
     * RETURNS    : 
     *      VOID
     */

    public void VRTestClick()
    {
        PlayClickAudio();
        if (usingVR)
        {
            DebugText = "Turns out you're already testing it. Otherwise you woudln't see this message.";
        }
        else
        {
            DebugText = "Cannot load VR scene. No HMD is connected.";
        }
    }

    /*
     * METHOD     : ExitClick()
     * DESCRIPTION: Called when the exit button is clicked. Will exit the application.
     * PARAMETERS : 
     *      VOID
     * RETURNS    : 
     *      VOID
     */

    public void ExitClick()
    {
        PlayClickAudio();
        PhotonController.Instance.Disconnect();
    }

    #endregion
}
