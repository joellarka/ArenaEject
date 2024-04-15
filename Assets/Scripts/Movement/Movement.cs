using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	private float maxSpeed = 25.0f;
	private Vector3 rawInput;
	private Vector3 velocity;
	private Vector3 currentVelocity;

	void Update()
	{
		rawInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
		float smoothingTime = rawInput.sqrMagnitude > 0 ? 0.12f : 0.08f;
		velocity = Vector3.SmoothDamp(velocity, rawInput * maxSpeed, ref currentVelocity, smoothingTime);

		if (velocity.sqrMagnitude > maxSpeed * maxSpeed)
		{
			velocity = velocity.normalized * maxSpeed;
		}

		transform.position += velocity * Time.deltaTime;

		if (velocity != Vector3.zero)
		{
			transform.rotation = Quaternion.LookRotation(velocity);
		}
	}
}
