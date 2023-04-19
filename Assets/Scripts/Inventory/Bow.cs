using System.Collections;
using System.Collections.Generic;
using Inventory;
using Player;
using UnityEngine;

public class Bow : MonoBehaviour, IWeapon
{
    public void Attack()
    {
        Debug.Log("Bow pew pew");
        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }
}
