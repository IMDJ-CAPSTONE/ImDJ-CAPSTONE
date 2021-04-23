/*! @file       : 	AbletonLinkTest2.cs
*   @author     : 	Ivan Granic, Jason Kassies, Div Dankahara, Mike Hilts
*   @date       : 	2021-03-01
*   @brief      : 	These files are unused
*/

using UnityEngine;
using UnityEngine.UI;

/*! <summary>
*  These files are unused
*  </summary>
*/
[RequireComponent(typeof(Text))]
public class LinkInfo : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
        //var numPeersPtr = System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(new AbletonLink.NumPeersCallbackDelegate(numPeers));
        //AbletonLink.Instance.setNumPeersCallback(numPeersPtr);
        //var tempoPtr = System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(new AbletonLink.TempoCallbackDelegate(tempo));
        //AbletonLink.Instance.setTempoCallback(tempoPtr);
    }

    // Update is called once per frame
    private void Update()
    {
        double beat, phase, tempo, time;
        int numPeers;
        AbletonLink.Instance.update(out beat, out phase, out tempo, out time, out numPeers);
        double quantum = AbletonLink.Instance.quantum();
        GetComponent<Text>().text = "Tempo:" + tempo + " Quantum:" + quantum + "\n"
            + "Beat:" + beat + " Phase:" + phase + "\n"
            + "NumPeers:" + numPeers;
    }

    //AbletonLink.NumPeersCallbackDelegate numPeers = peers => {
    //	Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name + " : " + peers);
    //};

    //AbletonLink.TempoCallbackDelegate tempo = bpm => {
    //	Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name + " : " + bpm);
    //};
}
