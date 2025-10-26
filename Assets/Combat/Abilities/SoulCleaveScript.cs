using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SoulCleaveScript : BaseAbilityScript
{
    public float damage = 50f;
    public float projectileSpeed = 6f;
    public GameObject soulCleaveProjectilePrefab;
    public SoulCleaveScript(string _abilityName, AbilitiesHandler _abilitiesHandler) : base(_abilityName, _abilitiesHandler)
    {
    }

    public override void OnAbilityCast()
    {
        if (cooldownActive)
        {
            return;
        }
      
        abilityHandler.StartCoroutine(Cooldown());
        SpawnProjectile();
        IEnumerator Cooldown()
        {
            cooldownActive = true;
            playerRef.equipSystem.currentWeaponObj.GetComponent<ScytheScript>().attackCooldownActive = true; 
            playerRef.equipSystem.currentWeaponObj.GetComponent<ScytheScript>().weaponAnimator.SetFloat("Speed", 2);
            playerRef.equipSystem.currentWeaponObj.GetComponent<ScytheScript>().weaponAnimator.SetTrigger("OnAttack");
           playerRef.equipSystem.currentWeaponObj.GetComponent<ScytheScript>().weaponAnimator.SetFloat("Speed", 1);
          slotObjRef.GetComponent<AbilitySlotScript>().ShowCooldownBar(cooldown);
            
            playerRef.equipSystem.currentWeaponObj.GetComponent<ScytheScript>().attackCooldownActive = false;
            yield return new WaitForSeconds(cooldown);
            slotObjRef.GetComponent<AbilitySlotScript>().HideCooldownBar();
            cooldownActive = false;
        }
    }

    public void SpawnProjectile()
    {
        GameObject soulCleaveProjectile = GameObject.Instantiate(soulCleaveProjectilePrefab, playerRef.directionOrigin.position + playerRef.directionOrigin.transform.right, playerRef.directionOrigin.rotation);
        soulCleaveProjectile.SetActive(true);
        soulCleaveProjectile.GetComponent<SoulCleaveProjectileScript>().refScript = this;
    }

    public override void UpdateAbilitySettings()
    {
        cooldown = abilityData.cooldown;
        soulCleaveProjectilePrefab = abilityHandler.abilityObjects.FirstOrDefault((x) => x.name == "SoulCleaveProjectile");
    }
}
