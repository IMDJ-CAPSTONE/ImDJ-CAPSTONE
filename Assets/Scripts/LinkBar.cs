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
public class LinkBar : MonoBehaviour
{
    private float prev = 1.0f;

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
        double q = AbletonLink.Instance.quantum();
        //Debug.Log ("" + (phase / q));
        float p = (float)(phase / q);
        if (p < prev)
            transform.Rotate(0, 0, 15.0f);
        prev = p;
    }
}
