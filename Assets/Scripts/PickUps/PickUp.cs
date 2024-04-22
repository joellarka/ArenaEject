using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickUpType { WEAPON, HEALTH, MOVESPEED }
public class PickUp : MonoBehaviour
{
    public PickUpType myType = PickUpType.WEAPON;

    public Weapon weaponPrefab;
}
