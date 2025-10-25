using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MegaHealAbilityScript : BaseAbilityScript
{
    public float healAmount = 40;
    public GameObject megaHealFX;
    public MegaHealAbilityScript(string _abilityName, AbilitiesHandler _abilitiesHandler) : base(_abilityName, _abilitiesHandler)
    {

    }

    public override void OnAbilityCast()
    {
        if (cooldownActive) { return; }
        megaHealFX = abilityHandler.abilityObjects.FirstOrDefault((x) => x.name == "MegaHealCircle");
        abilityHandler.StartCoroutine(Cooldown());
        abilityHandler.StartCoroutine(ActivateFX());
        IEnumerator Cooldown()
        {
            cooldownActive = true;
            slotObjRef.GetComponent<AbilitySlotScript>().ShowCooldownBar(cooldown);
            megaHealFX.SetActive(true);
            yield return new WaitForSeconds(cooldown);
            slotObjRef.GetComponent<AbilitySlotScript>().HideCooldownBar();
            cooldownActive = false;
        }
    }

    IEnumerator ActivateFX()
    {
        megaHealFX.SetActive(true);
        yield return new WaitForSeconds(0.683f);
        megaHealFX.SetActive(false);
    }

    public override void UpdateAbilitySettings()
    {
        cooldown = abilityData.cooldown;

    }
}
