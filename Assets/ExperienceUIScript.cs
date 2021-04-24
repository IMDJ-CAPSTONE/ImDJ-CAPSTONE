/*! @file       : 	ExperienceUIScript.cs
*   @author     : 	Ivan Granic, Jason Kassies
*   @date       : 	2021-04-01
*   @brief      : 	Contains the logic for the Experience UI
*/

using Lean.Gui;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static UserInstantiation;

/*! <summary>
*  Contains the logic for the Experience UI, specifically the main menu where every user
*  can change their resolution, enter/exit full screen mode and control volume with a slider
*  </summary>
*/
public class ExperienceUIScript : MonoBehaviour
{
    public GameObject openMenu;
    public LeanWindow window;
    public Dropdown dropdownMenu;
    public AudioMixer mixer;
    private Resolution[] resolutions;

    /*! <summary>
     *  This function get executed before anything else in this file
     *  it sets up the main menu, adding resolution options
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    private void Start()
    {
        if ((UserType)PhotonNetwork.LocalPlayer.CustomProperties["Type"] == UserType.Desktop)
        {
            openMenu.SetActive(false);
        }
        resolutions = Screen.resolutions;
        dropdownMenu.onValueChanged.AddListener(delegate
            {
                Screen.SetResolution(resolutions[dropdownMenu.value].width,
                                     resolutions[dropdownMenu.value].height, false, 50);
            });

        for (int i = 0; i < resolutions.Length; i++)
        {
            string tmpres = resolutions[i].width + " x " + resolutions[i].height;

            if (dropdownMenu.options.Contains(new Dropdown.OptionData(tmpres)) == false)
            {
                dropdownMenu.options[i].text = tmpres;
                dropdownMenu.value = i;
                dropdownMenu.options.Add(new Dropdown.OptionData(dropdownMenu.options[i].text));
            }
        }

        //check if the payler has old volume setting and set it
        mixer.SetFloat("MasterVol", PlayerPrefs.GetFloat("MasterVol"));
    }

    /*! <summary>
     *  This function get executed every frame, it checks if the user has the
     *  application selected and displays cursor accordingly
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && window.On == false)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            window.TurnOn();
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && window.On == true)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            window.TurnOff();
        }
    }

    /*! <summary>
     *  This function will close the program
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    public void exit()
    {
        Application.Quit();
    }

    /*! <summary>
     *  This function toggles the application being fullscreen
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    public void fullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    /*! <summary>
     *  This function get executed when the user moves the volume slider,
     *  it sets the volume level for the user
     *  </summary>
     *  <param name="sliderVal">a floting point number between zero and one</param>
     *  <returns>void</returns>
     */
    public void updateAudio(float sliderVal)
    {
        //the Mathf.Log10 is to convert the float from a linear value to logarithmic which makes it easier to control
        mixer.SetFloat("MasterVol", Mathf.Log10(sliderVal) * 20);
        PlayerPrefs.SetFloat("MasterVol", Mathf.Log10(sliderVal) * 20);
    }
}
