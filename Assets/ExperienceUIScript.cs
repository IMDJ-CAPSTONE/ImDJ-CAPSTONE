using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceUIScript : MonoBehaviour
{
    public GameObject GameMenu;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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

}
