using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[Tooltip("The target the camera will follow.")]
	[SerializeField] private GameObject target;

	[Tooltip("The target the camera will follow.")]
	[SerializeField] private Vector3 offset;

	[Tooltip("Sensitivity when moving camera with mouse.")]
	[SerializeField] private float sensitivity = 1;

	private void Update()
	{
		if (Input.GetMouseButton(0))
		{
			MouseMovement();
		}
		else if (Input.GetMouseButtonUp(0) && target != null)
		{
			transform.LookAt(target.transform);
		}
	}

	private void MouseMovement()
	{
		float horizontal = Input.GetAxisRaw("Mouse X");
		float vertical = Input.GetAxisRaw("Mouse Y");
		transform.Rotate(Vector3.up * horizontal * sensitivity);
		transform.Rotate(Vector3.right * -vertical * sensitivity);
		Vector3 rot = transform.rotation.eulerAngles;
		rot.z = 0f;
		transform.rotation = Quaternion.Euler(rot);
	}

	public void SetTarget(GameObject newTarget)
	{
		target = newTarget;
		transform.parent = target.transform;
		transform.localPosition = offset;

		transform.LookAt(target.transform);
	}
}
