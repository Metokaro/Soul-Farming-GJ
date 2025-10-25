using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class HealAbilityScript : BaseAbilityScript
{
    public float healAmountPerTick = 5;
    public float duration = 3;
    public GameObject particleEffects;
    float secondsPassedBy;
    public HealAbilityScript(string _abilityName, AbilitiesHandler _abilitiesHandler) : base(_abilityName, _abilitiesHandler)
    {
    }

    public override void OnAbilityCast()
    {
        if(cooldownActive) { return; }
        slotObjRef.GetComponent<AbilitySlotScript>().ShowCooldownBar(cooldown);
        particleEffects = abilityHandler.abilityObjects.FirstOrDefault((x) => x.name == "HealParticleEffects");
       particleEffects.SetActive(true);
        particleEffects.GetComponent<ParticleSystem>().Play();
        abilityHandler.StartCoroutine(Cooldown());
        abilityHandler.StartCoroutine(HealInterval());
        IEnumerator Cooldown()
        {
            cooldownActive = true;
            yield return new WaitForSeconds(cooldown);
            slotObjRef.GetComponent<AbilitySlotScript>().HideCooldownBar();
        
            cooldownActive = false;
        }
    }

    IEnumerator HealInterval()
    {
        for(int i = 0; i < duration; i++)
        {
            Debug.Log("Heal");
            yield return new WaitForSeconds(1);
        }
        particleEffects.SetActive(false);
    }


    public override void UpdateAbilitySettings()
    {
        cooldown = abilityData.cooldown;
    }

}
