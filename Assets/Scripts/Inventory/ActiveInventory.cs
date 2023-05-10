using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using Scene_Management;
using UnityEngine;

public class ActiveInventory : Singleton<ActiveInventory>
{
    private int activeSlotIndexNum = 0;

    private PlayerControls _playerControls;

    protected override void Awake()
    {
        base.Awake();
        _playerControls = new PlayerControls();
    }

    private void Start()
    {
        _playerControls.Inventory.Keyboard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());
        
    }

    public void EquipStartingWeapon()
    {
        ToggleActiveHighlight(0);
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void ToggleActiveSlot(int numValue)
    {
        ToggleActiveHighlight(numValue - 1);
    }

    private void ToggleActiveHighlight(int indexNum)
    {
        activeSlotIndexNum = indexNum;

        foreach (Transform inventorySlot in transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }
        
        transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);
        
        ChangeActiveWeapon();
    }

    private void ChangeActiveWeapon()
    {
        if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
        {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }

        var childTransform = transform.GetChild(activeSlotIndexNum);
        var inventorySlot = childTransform.GetComponentInChildren<InventorySlot>();
        var weaponInfo = inventorySlot.GetWeaponInfo();
        var weaponToSpawn = weaponInfo.weaponPrefab;
        
        if (weaponInfo == null)
        {
            ActiveWeapon.Instance.WeaponNull();
            return;
        }

        var newWeapon =
            Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform.position, Quaternion.identity);
        ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0,0,0);
        newWeapon.transform.parent = ActiveWeapon.Instance.transform;
        
        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }
}
