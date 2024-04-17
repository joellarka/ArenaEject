using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [HideInInspector] public bool appropriatlySpawned = false;
	[SerializeField] private float maxSpeed = 18.0f;
    [SerializeField] private float acceleration = 20f;
	private Vector3 rawInput;
    private Rigidbody rb;

    [HideInInspector] public int controllerIndex = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
	{
        PlayerMovement();
    }

	private void PlayerMovement()
	{
        if(appropriatlySpawned) rawInput = new Vector3(Input.GetAxisRaw($"P{controllerIndex}_Horizontal_Duo"), rb.velocity.y, Input.GetAxisRaw($"P{controllerIndex}_Vertical_Duo") * -1f);
        else rawInput = new Vector3(Input.GetAxisRaw($"Horizontal"), rb.velocity.y, Input.GetAxisRaw("Vertical")).normalized;

        // Acceleration
        if (rawInput != Vector3.zero)
        {
            
            rb.velocity += acceleration * maxSpeed * Time.deltaTime * rawInput;
            if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed) rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        else
        {
            // Deceleration
            rb.velocity -= acceleration * maxSpeed * Time.deltaTime * rb.velocity.normalized;
            if (rb.velocity.sqrMagnitude < 0.01) rb.velocity = Vector2.zero;
        }
    }
}
