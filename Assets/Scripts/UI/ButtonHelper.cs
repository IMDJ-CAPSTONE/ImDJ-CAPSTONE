/*
 *  FILE          :	ButtonHoverSound.cs
 *  PROJECT       :	ImDJ Capstone Project 
 *  PROGRAMMER    :	Michael Hilts - 5377643
 *  FIRST VERSION :	Feb 6, 2021
 *  DESCRIPTION   : This file contains the ButtonHoverSound class which is responsible
 *                  for playing the audio from the gameobjects AudioSource whenever
 *                  the user mouses over the button it is attached to.
 */

#region Resources

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#endregion

[RequireComponent(typeof(AudioSource))]
public class ButtonHelper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region Fields

    private AudioSource hoverAudio;  // this gameobject attached AudioSource

    #endregion

    #region MonoBehaviour Callbacks

    /*
     * METHOD     : Start()
     * DESCRIPTION: Called before the first frame update. This will get reference to
     *              the attached AudioSource component. 
     * PARAMETERS : 
     *      VOID
     * RETURNS    : 
     *      VOID
     */

    private void Start()
    {
        hoverAudio = GetComponent<AudioSource>();
    }

    #endregion

    #region Public Methods

    /*
     * METHOD     : PlayHoverAudio()
     * DESCRIPTION: Public method to allow other classes to play the audio. 
     * PARAMETERS : 
     *      VOID
     * RETURNS    : 
     *      VOID
     */

    public void PlayHoverAudio()
    {
        hoverAudio.Play();
    }

    #endregion

    #region Interface Implementations

    /*
     * METHOD     : OnPointerEnter()
     * DESCRIPTION: The implementation of IPointerEnterHandler. This will be called whenever
     *              the mouse enters this UI elements bounds. It will play the audio from
     *              the attached AudioSource.
     * PARAMETERS : 
     *      VOID
     * RETURNS    : 
     *      VOID
     */

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayHoverAudio();
        EventSystem.current.SetSelectedGameObject(gameObject);
        if (TabControl.current != null)
        {
            TabControl.current.UsingMouse = true;
        }
    }

    /*
     * METHOD     : OnPointerExit()
     * DESCRIPTION: The implementation of IPointerExitHandler. This will be called whenever
     *              the mouse exits this UI elements bounds. It will deselect the button.
     * PARAMETERS : 
     *      VOID
     * RETURNS    : 
     *      VOID
     */

    public void OnPointerExit(PointerEventData eventData)
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        if (TabControl.current != null)
        {
            TabControl.current.UsingMouse = false;
        }
    }

    #endregion
}
