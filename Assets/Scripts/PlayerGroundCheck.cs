/*! @file       : 	PerformerUserUIController.cs
*   @author     : 	Ivan Granic, Jason Kassies
*   @date       : 	2021-03-01
*   @brief      : 	Contains the collision logic for the players player object
*/

using UnityEngine;

/*! <summary>
*  Contains the collision logic for the players player object
*  </summary>
*/
public class PlayerGroundCheck : MonoBehaviour
{
    private PlayerController playerController;

    /*! <summary>
     *  This function sets the playerController
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    /*! <summary>
     *  This function checks the colliding object when they collide
     *  </summary>
     *  <param name="other">the other object that is being collided with</param>
     *  <returns>void</returns>
     */
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerController.gameObject) return;
        playerController.SetGroundedState(true);
    }

    /*! <summary>
     *  This function is called when the collision ends
     *  </summary>
     *  <param name="other">the other object that is being collided with</param>
     *  <returns>void</returns>
     */
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerController.gameObject) return;
        playerController.SetGroundedState(false);
    }

    /*! <summary>
     *  This function is called while intersecting with another object
     *  </summary>
     *  <param name="other">the other object that is being collided with</param>
     *  <returns>void</returns>
     */
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == playerController.gameObject) return;
        playerController.SetGroundedState(true);
    }

    /*! <summary>
     *  This function checks the colliding object when they collide
     *  </summary>
     *  <param name="collision">the other object that is being collided with</param>
     *  <returns>void</returns>
     */
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == playerController.gameObject) return;
        playerController.SetGroundedState(true);
    }

    /*! <summary>
     *  This function is called when the collision ends
     *  </summary>
     *  <param name="collision">the other object that is being collided with</param>
     *  <returns>void</returns>
     */
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == playerController.gameObject) return;
        playerController.SetGroundedState(false);
    }

    /*! <summary>
     *  This function is called while intersecting with another object
     *  </summary>
     *  <param name="collision">the other object that is being collided with</param>
     *  <returns>void</returns>
     */
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == playerController.gameObject) return;
        playerController.SetGroundedState(true);
    }
}
