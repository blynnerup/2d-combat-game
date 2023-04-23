using Inventory;
using Player;
using UnityEngine;

public class Staff : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    private void Update()
    {
        MouseFollowWithOffset();
    }

    public void Attack()
    {
        Debug.Log("Staff pew pew");
    }
    
    private void MouseFollowWithOffset()
    {
        var playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);
        var mousePos = Input.mousePosition;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        
        ActiveWeapon.Instance.transform.rotation = mousePos.x < playerScreenPoint.x ? Quaternion.Euler(0, -180, angle) : Quaternion.Euler(0, 0, angle);
    }
    
    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
}
