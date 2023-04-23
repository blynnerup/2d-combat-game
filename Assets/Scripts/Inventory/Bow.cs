using System.Collections;
using System.Collections.Generic;
using Inventory;
using Player;
using UnityEngine;

public class Bow : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    public void Attack()
    {
        Debug.Log("Bow pew pew");
    }
    
    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
}
