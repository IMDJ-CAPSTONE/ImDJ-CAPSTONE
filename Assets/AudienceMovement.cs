using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceMovement : MonoBehaviour
{
	[Tooltip("The speed the character will move.")]
	[SerializeField] private float moveSpeed = 10f;

	[Tooltip("The speed the character will turn.")]
	[SerializeField] private float turnSpeed = 100f;

	private Transform _transform;
	private Rigidbody _rigidbody;
	private Vector3 _input;
	private Vector3 _rotationVector;

	private void Awake()
	{
		_transform = transform;
		_rigidbody = GetComponent<Rigidbody>();
		_rotationVector = new Vector3(0f, turnSpeed, 0f);
	}

	private void Update()
	{
		GetInput();
	}

	private void FixedUpdate()
	{
		Move();
		Turn();
	}

	private void GetInput()
	{
		_input = Vector3.zero;
		_input.x = Input.GetAxis("Horizontal");
		_input.z = Input.GetAxis("Vertical");
	}

	private void Move()
	{
		if (_input.z != Vector3.zero.z)
		{
			Vector3 movement = _rigidbody.position + _transform.forward * _input.z * moveSpeed * Time.deltaTime;
			_rigidbody.MovePosition(movement);
		}
	}

	private void Turn()
	{
		if (_input.x != Vector3.zero.x)
		{
			Quaternion deltaRotation = Quaternion.Euler(_input.x * _rotationVector * Time.fixedDeltaTime);
			_rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
		}
	}
}
