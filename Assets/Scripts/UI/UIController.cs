/*
 *  FILE          :	UIController.cs
 *  PROJECT       :	ImDJ Capstone Project 
 *  PROGRAMMER    :	Michael Hilts - 5377643
 *  FIRST VERSION :	Feb 6, 2021
 *  DESCRIPTION   : This file contains the UIController class which contains all the on click
 *                  methods for the UI buttons.
 */

#region Resources

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

[RequireComponent(typeof(AudioSource))]
public class UIController : MonoBehaviour
{
    #region fields

    [Tooltip("The build index for the VR test scene.")]
    [SerializeField]
    private int VRTestSceneIndex = 1;

    private AudioSource clickAudio;
    private TextMeshProUGUI debugText;  // the debug text to print messages to the screen

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

    #endregion

    #region Button Click Handlers

    /*
     * METHOD     : PhotonTestClick()
     * DESCRIPTION: Called when the photon test button is clicked. Will run the photon test
     *              to attempt to connect to the photon servers.
     * PARAMETERS : 
     *      VOID
     * RETURNS    : 
     *      VOID
     */

    public void PhotonTestClick()
    {
        PlayClickAudio();
        PhotonController.Instance.Connect();
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
        SceneManager.LoadScene(VRTestSceneIndex);
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

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit(0);
        #endif
    }

    #endregion
}
