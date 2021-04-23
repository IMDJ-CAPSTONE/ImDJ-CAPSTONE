/*! @file       : 	AbletonLinkTest2.cs
*   @author     : 	Ivan Granic, Jason Kassies, Div Dankahara, Mike Hilts
*   @date       : 	2021-03-01
*   @brief      : 	These files are unused
*/

using UnityEngine;

/*! <summary>
*  These files are unused
*  </summary>
*/
public class LinkBeat : MonoBehaviour
{
    private AbletonLink link;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        double beat, phase, tempo, time;
        int numPeers;
        AbletonLink.Instance.update(out beat, out phase, out tempo, out time, out numPeers);
        //Debug.Log ("beat: " + beat + " phase:" + phase);
        float fphase = (float)phase;
        fphase = fphase - Mathf.Floor(fphase);
        transform.localScale = new Vector3(fphase, fphase, fphase);

        float tempo01 = 1f - Mathf.Clamp01((float)AbletonLink.Instance.tempo() / 240.0f);

        Color c = Color.HSVToRGB(tempo01, 1, 1);
        GetComponent<Renderer>().material.SetColor("_Color", c);
    }
}
