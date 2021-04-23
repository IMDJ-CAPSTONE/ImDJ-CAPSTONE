/*! @file       : 	ButtonHelper.cs
*   @author     : 	Michael Hilts, Ivan Granic
*   @date       : 	2021-02-06
*   @brief      : 	This file contains the ButtonHelper class that will access pointer events
*/

#region Resources

using UnityEngine;
using UnityEngine.EventSystems;

#endregion Resources

/*! <summary>
*  This file contains the ButtonHelper class which implements both
*  IPointerEnterHandler and IPointerExitHandler to provide access to
*  the pointer enter event for this button as well as the pointer exit event.
*  </summary>
*/
[RequireComponent(typeof(AudioSource))]
public class ButtonHelper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region Fields

    private AudioSource hoverAudio;  // this gameobject attached AudioSource

    #endregion Fields

    #region MonoBehaviour Callbacks

    /*! <summary>
     *  Called before the first frame update. This will get reference to the attached AudioSource component.
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    private void Start()
    {
        hoverAudio = GetComponent<AudioSource>();
    }

    #endregion MonoBehaviour Callbacks

    #region Public Methods

    /*! <summary>
     *  Public method to allow other classes to play the audio.
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    public void PlayHoverAudio()
    {
        hoverAudio.Play();
    }

    #endregion Public Methods

    #region Interface Implementations

    /*! <summary>
     *  The implementation of IPointerEnterHandler. This will be called whenever the mouse enters
     *  this UI elements bounds. It will play the audio from the attached AudioSource.
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
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

    /*! <summary>
     *  The implementation of IPointerExitHandler. This will be called whenever
     *  the mouse exits this UI elements bounds. It will deselect the button.
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
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

    #endregion Interface Implementations
}
