using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    [HideInInspector] public bool appropriatlySpawned = false;
    [HideInInspector] public int controllerIndex = 1;
    [SerializeField] [Range(0.3f, 3.2f)] private float rotationSpeedRads = 1;


    private void Update()
    {
        PlayerLook();
    }

    private void PlayerLook()
    {
        Vector3 input = Vector3.zero;

        if (appropriatlySpawned) input = new Vector3(Input.GetAxisRaw($"P{controllerIndex}_Aim_Horizontal"), 0, Input.GetAxisRaw($"P{controllerIndex}_Aim_Vertical") * -1f);
        else input = new Vector3(Input.GetAxisRaw($"Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        if(input.sqrMagnitude < 0.5f)
        {
            input = new Vector3(Input.GetAxisRaw($"P{controllerIndex}_Horizontal_Duo"), 0, Input.GetAxisRaw($"P{controllerIndex}_Vertical_Duo") * -1f).normalized;
        }
        else
        {
            input.Normalize();
        }

        if(input != Vector3.zero)
        {
            // Instant roation
            /*
            float angle = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, angle, 0);
            */


            float angle = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeedRads);
        }
    }
}