using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RonjaDebug : MonoBehaviour
{
    private void Awake()
    {
        if (PlayerData.playerToControllerBinding == null) return;

        foreach (var t in PlayerData.playerToControllerBinding)
        {
            Debug.Log(t);
        }
        
    }
}
