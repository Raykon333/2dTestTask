using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public string[] Items;
    public int[] Amounts;
    public Vector3 CharacterPosition;
    public float CharacterHealth;
    public Vector3[] EnemyPositions;
    public float[] EnemyHealth;
    public PickupItem[] Pickups;
    public Vector3[] PickupPositions;
}
