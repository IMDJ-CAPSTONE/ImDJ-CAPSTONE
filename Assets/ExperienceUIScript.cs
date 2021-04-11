using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

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
                                   resolutions[dropdownMenu.value].height, false); });
        
        
        for (int i = 0; i < resolutions.Length; i++)
        {
            //there was an issue with this creating multiples of some resolutions but the bug dissapeared
            //either way this should prevent it, if it crops up again
            ///if(!dropdownMenu.options.Contains(new Dropdown.OptionData(dropdownMenu.options[i].text)))
            dropdownMenu.options[i].text = resolutions[i].width + " x " + resolutions[i].height;
            dropdownMenu.value = i;
            dropdownMenu.options.Add(new Dropdown.OptionData(dropdownMenu.options[i].text));
            
        }

        //check if the payler has old volume setting and set it
        mixer.SetFloat("MasterVol", PlayerPrefs.GetFloat("MasterVol"));
    }

    
    void Update()
    {
        
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
