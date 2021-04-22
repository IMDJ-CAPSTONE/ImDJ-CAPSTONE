/*
 *  FILE          :	TabControl.cs
 *  PROJECT       :	ImDJ Capstone Project 
 *  PROGRAMMER    :	Michael Hilts - 5377643
 *  FIRST VERSION :	Feb 6, 2021
 *  DESCRIPTION   : This file contains the TabControl script which can be placed on
 *                  a ui game object to allow the user to tab through any selectables
 *                  in either itself or any children in its directory structure.
 *                  NOTE: Only one TabControl should be active at a time as current will
 *                  only reference the most recently activated.
 */

#region Resources

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#endregion

public class TabControl : MonoBehaviour
{
    #region Fields

    public static TabControl current;       // most recently activated TabControl

    private List<Selectable> selectables;   // a list of all selectables under this game object   
    private int selected = -1;              // the currently selected ui element, -1 means none

    #endregion

    #region Properties

    public bool UsingMouse { get; set; } = false;

    #endregion

    #region MonoBehaviour Callbacks

    /* METHOD     : Start()
     * DESCRIPTION: Called before the first frame update. This will instantiate the list
     *              of selectables, check this object for a selectable and then make the
     *              call to populate said list from all child gameobjects.
     * PARAMETERS : 
     *      VOID
     * RETURNS    : 
     *      VOID
     */
    void Start()
    {
        current = this;

        selectables = new List<Selectable>();
        Selectable selectable = transform.GetComponent<Selectable>();
        if (selectable != null)
        {
            selectables.Add(selectable);
        }
        PopulateList(transform);
    }

    /* METHOD     : Update()
     * DESCRIPTION: This method is called once per frame. It will check for user input.
     * PARAMETERS : 
     *      VOID
     * RETURNS    : 
     *      VOID
     */
    void Update()
    {
        GetInput();
    }

    /* METHOD     : Update()
     * DESCRIPTION: This method is called when the object is enabled, and will set
     *              Current to this TabControl.
     * PARAMETERS : 
     *      VOID
     * RETURNS    : 
     *      VOID
     */
    private void OnEnable()
    {
        current = this;
    }

    #endregion

    #region Private Methods

    /* METHOD     : PopulateList()
     * DESCRIPTION: This method will check each child of the transform passed in for a selectable
     *              component and if so it will add it to the list. If the child transform
     *              has children it will recursively call itself on each to find all selectables
     *              in the directory structure below the gameobject with this script attached.
     * PARAMETERS : 
     *      Transform trans: The transform whose children should be searched through for selectables.
     * RETURNS    : 
     *      VOID
     */
    private void PopulateList(Transform trans)
    {
        // loop through each child of this transform
        foreach (Transform child in trans)
        {
            // check if this child has a selectable
            Selectable selectable = child.GetComponent<Selectable>();
            if (selectable != null)
            {
                selectables.Add(selectable);
            }

            // if this child has children, call recursively
            if (child.childCount > 0)
            {
                PopulateList(child);
            }
        }
    }

    /* METHOD     : GetInput()
     * DESCRIPTION: This method will be called from update and will check if the user pressed the tab
     *              key. If so it will move the selection to the next selectable in the list.
     * PARAMETERS : 
     *      VOID
     * RETURNS    : 
     *      VOID
     */
    private void GetInput()
    {
        // if unselected in some other way set selected accordingly
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            selected = -1;
        }

        if (!UsingMouse)
        {
            // if user pressed tab
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                // if none selected or at last selectable, select the first
                if (selected == -1 || selected >= selectables.Count - 1)
                {
                    selected = 0;
                }

                // otherwise incrememnt selected
                else
                {
                    selected += 1;
                }

                // set new selection
                selectables[selected].Select();

                // if selection has audio, play it
                ButtonHelper audio = selectables[selected].gameObject.GetComponent<ButtonHelper>();
                if (audio != null)
                {
                    audio.PlayHoverAudio();
                }
            }
        }

        // if user presses enter to click button play click sound
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (selected >= 0)
            {
                GameObject.FindGameObjectWithTag("UI").GetComponent<UIController>().PlayClickAudio();
            }
        }
    }

    #endregion
}
