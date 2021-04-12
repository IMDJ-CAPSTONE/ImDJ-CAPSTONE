using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementBeatController : MonoBehaviour
{
    private int count;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setBeat(float tempo)
    {
        float speed = tempo / 60;
        foreach (var anim in this.GetComponentsInChildren<Animator>())
        {
            anim.SetFloat("BeatSpeed", speed);
            anim.SetTrigger("BeatEvent");
        }
    }
}
