/*! @file       : 	PerformerUserUIController.cs
*   @author     : 	Ivan Granic, Mike Hilts
*   @date       : 	2021-03-01
*   @brief      : 	Contains the logic for the player controller
*/

using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

/*! <summary>
*  Contains the logic for the player controller
*  </summary>
*/
public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject cameraHolder;
    [SerializeField] private float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;

    private int itemIndex;
    private int previousItemIndex = -1;
    private Rigidbody rb;

    private float verticalLookRotation;
    public bool grounded;
    private Vector3 smoothMoveVelocity;
    private Vector3 moveAmount;
    private PhotonView PV;

    /*! <summary>
     *  This function get executed when the script instance is being loaded
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
    }

    /*! <summary>
     *  This function get executed before anything else in this file
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    private void Start()
    {
        if (PV.IsMine)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
        }
    }

    /*! <summary>
     *  This function get executed every frame, if a key is being pressed it will respond accordingly
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    private void Update()
    {
        if (!PV.IsMine) return;
        Look();
        Move();
        Jump();

        if (Input.GetKeyDown(KeyCode.Q)) EndGame();
    }

    /*! <summary>
     *  This function applies the movements to the player object in game
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    private void FixedUpdate()
    {
        if (!PV.IsMine) return;
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    /*! <summary>
     *  This function moves the player based on keyboard input
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    private void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
    }

    /*! <summary>
     *  This function makes the player jump if the space key is pressed
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(transform.up * jumpForce);
        }
    }

    /*! <summary>
     *  This function moves the camera angle of the player based on mouse input
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    private void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);
        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    /*! <summary>
     *  This function sets the public variable to the passed in value
     *  </summary>
     *  <param name="_grounded">the new value for the variable grounded</param>
     *  <returns>void</returns>
     */
    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }

    /*! <summary>
     *  This function gets called to exit the application
     *  </summary>
     *  <param name="none"></param>
     *  <returns>void</returns>
     */
    private void EndGame()
    {
        if (PhotonNetwork.IsMasterClient) PhotonNetwork.DestroyAll();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }
}
