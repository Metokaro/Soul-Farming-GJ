using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipSystem
{
    public GameObject currentWeaponObj;
    public Transform weaponParent;
    public PlayerController playerControllerRef;
    public PlayerEquipSystem(PlayerController _playerController, Transform _weaponParent)
    {
        playerControllerRef = _playerController;
        weaponParent = _weaponParent;
    }
    public void EquipNewWeapon(WeaponDataTemplate newWeaponData)
    {
        if(currentWeaponObj != null)
        {
            GameObject.Destroy(currentWeaponObj);

        }

        GameObject newWeapon = GameObject.Instantiate(newWeaponData.weaponPrefab, weaponParent);
        currentWeaponObj = newWeapon;
        currentWeaponObj.GetComponent<BaseWeaponScript>().weaponData = newWeaponData;
        currentWeaponObj.GetComponent<BaseWeaponScript>().playerRef = playerControllerRef;
        currentWeaponObj.GetComponent<BaseWeaponScript>().OnEquip();
    }
}
