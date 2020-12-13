using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    [SerializeField]
    float baseProjectileCount = 10;

    [SerializeField]
    int shotsPerRow = 20;

    [SerializeField]
    int firingSpreadEulerAngle = 45;

    [SerializeField]
    float cooldown = 1;

    
}
