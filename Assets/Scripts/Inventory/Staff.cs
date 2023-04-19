using System.Collections;
using System.Collections.Generic;
using Inventory;
using Player;
using UnityEngine;

public class Staff : MonoBehaviour, IWeapon
{
    public void Attack()
    {
        Debug.Log("Staff pew pew");
        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }
}
