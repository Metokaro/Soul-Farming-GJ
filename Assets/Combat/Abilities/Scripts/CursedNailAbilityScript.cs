using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedNailAbilityScript : BaseAbilityScript
{
    public CursedNailAbilityScript(string _abilityName, AbilitiesHandler _abilitiesHandler) : base(_abilityName, _abilitiesHandler)
    {
    }

    public override void OnAbilityCast()
    {
        if (cooldownActive)
        {
            return;
        }

        abilityHandler.StartCoroutine(Cooldown());
        playerRef.equipSystem.currentWeaponObj.GetComponent<NailgunScript>().ShootCursedNail();
        IEnumerator Cooldown()
        {
            cooldownActive = true;
            slotObjRef.GetComponent<AbilitySlotScript>().ShowCooldownBar(cooldown);
            yield return new WaitForSeconds(cooldown);
            slotObjRef.GetComponent<AbilitySlotScript>().HideCooldownBar();
            cooldownActive = false;
        }
    }
    public override void UpdateAbilitySettings()
    {
        cooldown = abilityData.cooldown;
    }
}
