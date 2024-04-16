using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int playerIndex = 1;
    public int lives = 3;
    public bool alive { get => lives > 0; }
}
