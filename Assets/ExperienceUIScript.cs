using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Linq;

public class ExperienceUIScript : MonoBehaviour
{
    public GameObject GameMenu;

    Resolution[] resolutions;
    public Dropdown dropdownMenu;
    public AudioMixer mixer;

    //volume slider link:
    ///https://gamedevbeginner.com/the-right-way-to-make-a-volume-slider-in-unity-using-logarithmic-conversion/ 

    void Start()
    {
        resolutions = Screen.resolutions;
        dropdownMenu.onValueChanged.AddListener(delegate 
            { Screen.SetResolution(resolutions[dropdownMenu.value].width, 
                                   resolutions[dropdownMenu.value].height, false, 50); });
        
        
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


    public void exit()
    {
        Application.Quit();
    }

    public void fullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void updateAudio(float sliderVal)
    {
        //the Mathf.Log10 is to convert the float from a linear value to logarithmic which makes it easier to control
        mixer.SetFloat("MasterVol", Mathf.Log10(sliderVal) * 20);
        PlayerPrefs.SetFloat("MasterVol", Mathf.Log10(sliderVal) * 20);
    }

}
