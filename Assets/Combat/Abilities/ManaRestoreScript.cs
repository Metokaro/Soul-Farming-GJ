using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ManaRestoreScript : BaseAbilityScript
{
    public float restorePerTick = 5;
    public float duration = 3;
    public GameObject particleEffects;
    float secondsPassedBy;
    public ManaRestoreScript(string _abilityName, AbilitiesHandler _abilitiesHandler) : base(_abilityName, _abilitiesHandler)
    {
    }

    public override void OnAbilityCast()
    {
        if(cooldownActive) { return; }
        particleEffects = abilityHandler.abilityObjects.FirstOrDefault((x) => x.name == "ManaParticleEffects");
        abilityHandler.StartCoroutine(Cooldown());
        abilityHandler.StartCoroutine(ManaRestoreInterval());
        
        IEnumerator Cooldown()
        {
            slotObjRef.GetComponent<AbilitySlotScript>().ShowCooldownBar(cooldown);
            cooldownActive = true;
            yield return new WaitForSeconds(cooldown);
            slotObjRef.GetComponent<AbilitySlotScript>().HideCooldownBar();
            cooldownActive = false;
        }
    }

    IEnumerator ManaRestoreInterval()
    {
        particleEffects.SetActive(true);
        for (int i = 0; i < duration; i++)
        {
            playerRef.mana += Mathf.Clamp(restorePerTick, 0, playerRef.maxMana - playerRef.mana); ;
            playerRef.playerScreenRef.UpdateManaBar();
            yield return new WaitForSeconds(1);
        }
        particleEffects.SetActive(false);
    }

    public override void UpdateAbilitySettings()
    {
        cooldown = abilityData.cooldown;
    }
}
