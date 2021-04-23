/*! @file       : 	UIController.cs
*   @author     : 	Ivan Granic, Mike Hilts
*   @date       : 	2021-02-06
*   @brief      : 	This file contains the UIController class which contains all the on click methods for the UI buttons.
*/

#region Resources

using ExitGames.Client.Photon;
using Photon.Pun;
using TMPro;
using UnityEngine;

#endregion Resources

/*! <summary>
*  This file contains the UIController class which contains all the on click methods for the UI buttons.
*  </summary>
*/
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

    #endregion fields

    #region Properties

    /*! <summary>
    *  allows printing of messages to the onscreen debug text
    *  </summary>
    */

    public string DebugText
    {
        set
        {
            debugText.text = value;
        }
    }

    #endregion Properties

    #region MonoBehaviour Callbacks

    /*! <summary>
     *  Called before the first frame update. This will get a reference to the debug text.
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
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

    #endregion MonoBehaviour Callbacks

    #region Public Methods

    /*! <summary>
     *  Public method to allow other classes to play the audio.
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    public void PlayClickAudio()
    {
        clickAudio.Play();
    }

    /*! <summary>
     *  Public method to allow other classes to exit the application.
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit(0);
#endif
    }

    #endregion Public Methods

    #region Button Click Handlers

    /*! <summary>
     *  Called when the PeformerUserStart buttton is clicked. Will run the photon test
     *  to attempt to connect to the photon servers.
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    public void PeformerUserStart()
    {
        PlayClickAudio();
        PhotonController.Instance.Connect();
        Hashtable hash = new Hashtable();
        hash.Add("Type", UserInstantiation.UserType.Performer);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    /*! <summary>
     *  Called when the PeformerUserStart buttton is clicked. Will run the photon test
     *  to attempt to connect to the photon servers.
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    public void DesktopUserStart()
    {
        PlayClickAudio();
        PhotonController.Instance.Connect();
        Hashtable hash = new Hashtable();
        hash.Add("Type", UserInstantiation.UserType.Desktop);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    /*! <summary>
     *  Called when the VR test button is clicked. Will take the user to the VR test scene.
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
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

    /*! <summary>
     *  Called when the exit button is clicked. Will exit the application.
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    public void ExitClick()
    {
        PlayClickAudio();
        PhotonController.Instance.Disconnect();
    }

    #endregion Button Click Handlers
}
