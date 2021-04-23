/*! @file       : 	UIPointer.cs
*   @author     : 	Ivan Granic, Jason Kassies, Div Dankahara, Mike Hilts
*   @date       : 	2021-03-11
*   @brief      : 	This file is unused as the functionality got cut, but it is included
*                   so that it can be implemented at a later date
*/

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.Extras;

public class UIPointer : MonoBehaviour
{
    private SteamVR_LaserPointer laserPointer;

    private void Start()
    {
        laserPointer = GetComponent<SteamVR_LaserPointer>();
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;
    }

    public void PointerClick(object sender, PointerEventArgs e)
    {
        Button button = e.target.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.Invoke();
        }
    }

    public void PointerInside(object sender, PointerEventArgs e)
    {
        Button button = e.target.GetComponent<Button>();
        if (button != null)
        {
            button.GetComponent<ButtonHelper>().OnPointerEnter(new PointerEventData(EventSystem.current));
        }
    }

    public void PointerOutside(object sender, PointerEventArgs e)
    {
        Button button = e.target.GetComponent<Button>();
        if (button != null)
        {
            button.GetComponent<ButtonHelper>().OnPointerExit(new PointerEventData(EventSystem.current));
        }
    }
}
