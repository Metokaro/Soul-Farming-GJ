using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeaponScript : MonoBehaviour
{
    [HideInInspector]public float calculatedDamage;
    [HideInInspector] public float calculatedWeaponSpeed;
    [HideInInspector]public PlayerController playerRef;
    [HideInInspector] public WeaponDataTemplate weaponData;
    [HideInInspector] public Animator weaponAnimator;

    public abstract void Attack();
    public virtual void OnEquip()
    {
        playerRef.pauseRotation = false;
        calculatedDamage = weaponData.DefaultDamage * playerRef.playerStats.powerMultiplier;
        calculatedWeaponSpeed = weaponData.DefaultWeaponSpeed * playerRef.playerStats.atkSpeedMultiplier;
      weaponAnimator=  GetComponent<Animator>();
    }
    public virtual void WhileHolding()
    {
        playerRef.MouseFaceDirection(out var mousePos, out float angle);
        playerRef.directionOrigin.transform.eulerAngles = new(0, 0, angle);
        float rotateDiff = (playerRef.spriteRenderer.flipX) ? -1 : 1;
        playerRef.equipSystem.weaponParent.transform.localScale = new(1, rotateDiff, 1);
    }
    public virtual void OnUnequip(){
        
    }
    public virtual void CastAbility()
    {

    }
}
