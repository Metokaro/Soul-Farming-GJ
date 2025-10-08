using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeaponScript : MonoBehaviour
{
    [HideInInspector]public float calculatedDamage;
    [HideInInspector] public float calculatedWeaponSpeed;
    public PlayerController playerRef;
    public WeaponDataTemplate weaponData;
    [HideInInspector] public Animator weaponAnimator;

    public abstract void Attack();
    public virtual void OnEquip()
    {
        calculatedDamage = weaponData.DefaultDamage * playerRef.playerStats.powerMultiplier;
        calculatedWeaponSpeed = weaponData.DefaultWeaponSpeed * playerRef.playerStats.atkSpeedMultiplier;
      weaponAnimator=  GetComponent<Animator>();
    }
    public virtual void OnUnequip(){
        
    }
    public virtual void CastAbility()
    {

    }
}
