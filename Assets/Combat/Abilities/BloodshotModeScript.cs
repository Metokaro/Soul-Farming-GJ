using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BloodshotModeScript : BaseAbilityScript
{
    public float modeDuration = 3f;

    public GameObject bloodshotProjectilePrefab;
    public BloodshotModeScript(string _abilityName, AbilitiesHandler _abilitiesHandler) : base(_abilityName, _abilitiesHandler)
    {
    }

    public override void OnAbilityCast()
    {
       if(cooldownActive)
        {
            return;
        }

        abilityHandler.StartCoroutine(Cooldown());
        abilityHandler.StartCoroutine(ModeDuration());
        IEnumerator Cooldown()
        {
            cooldownActive = true; 
            slotObjRef.GetComponent<AbilitySlotScript>().ShowCooldownBar(cooldown);
            yield return new WaitForSeconds(cooldown);
            slotObjRef.GetComponent<AbilitySlotScript>().HideCooldownBar();
            cooldownActive = false;
        }
        IEnumerator ModeDuration()
        {
            playerRef.equipSystem.currentWeaponObj.GetComponent<BloodyChainsawScript>().bloodshotModeOn = true;
            yield return new WaitForSeconds(modeDuration);
            playerRef.equipSystem.currentWeaponObj.GetComponent<BloodyChainsawScript>().bloodshotModeOn = false;
        }
    }



    public override void UpdateAbilitySettings()
    {
        cooldown = abilityData.cooldown;
        bloodshotProjectilePrefab = abilityHandler.abilityObjects.FirstOrDefault((x) => x.name == "BloodshotProjectile");
        playerRef.equipSystem.currentWeaponObj.GetComponent<BloodyChainsawScript>().bloodShotProjectilePrefab = bloodshotProjectilePrefab;
    }

}
