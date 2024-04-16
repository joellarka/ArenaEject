using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    [HideInInspector] public bool appropriatlySpawned = false;
    [HideInInspector] public int controllerIndex = 1;
    [SerializeField] private float rotationSpeed = 30f;


    private void Update()
    {
        PlayerLook();
    }

    private void PlayerLook()
    {
        Vector3 input = Vector3.zero;

        if (appropriatlySpawned) input = new Vector3(Input.GetAxisRaw($"P{controllerIndex}_Horizontal_Duo"), 0, Input.GetAxisRaw($"P{controllerIndex}_Vertical_Duo") * -1f)/*.normalized*/;
        else input = new Vector3(Input.GetAxisRaw($"Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        transform.LookAt(input);
    }
}