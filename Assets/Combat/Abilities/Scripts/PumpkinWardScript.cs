using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PumpkinWardScript : BaseAbilityScript
{
    public GameObject pumpkinWard1;
    public GameObject pumpkinWard2;
    public int hitsPerWard = 3;
    public int totalHitsPerWard;
    public int totalHits;
    public float wardDuration = 8;
    public PumpkinWardScript(string _abilityName, AbilitiesHandler _abilitiesHandler) : base(_abilityName, _abilitiesHandler)
    {
    }

    public override void OnAbilityCast()
    {
        if(cooldownActive)
        {
            return;
        }
        totalHits = 0;
        playerRef.onTakeDamageFunction = ProtectPlayer;
        totalHitsPerWard = hitsPerWard * 2;
        abilityHandler.StartCoroutine(Cooldown());
        abilityHandler.StartCoroutine(Duration());
        pumpkinWard1 = abilityHandler.abilityObjects.FirstOrDefault((x) => x.name == "PumpkinWardParent1");
        pumpkinWard2 = abilityHandler.abilityObjects.FirstOrDefault((x) => x.name == "PumpkinWardParent2");
        pumpkinWard1.SetActive(true);
        pumpkinWard2.SetActive(true);
       
        IEnumerator Cooldown()
        {
            cooldownActive = true;
            slotObjRef.GetComponent<AbilitySlotScript>().ShowCooldownBar(cooldown);
            
            yield return new WaitForSeconds(cooldown);
            slotObjRef.GetComponent<AbilitySlotScript>().HideCooldownBar();
            cooldownActive = false;
        }
        IEnumerator Duration()
        {

            yield return new WaitForSeconds(wardDuration);
            pumpkinWard1.SetActive(false);
            pumpkinWard2.SetActive(false);
            playerRef.onTakeDamageFunction = null;
        }
    }
    


    public void ProtectPlayer()
    {
        if (totalHits < totalHitsPerWard)
        {
            totalHits += 1;
            playerRef.canTakeDamage = false;
        }
        else
        {
            playerRef.canTakeDamage = true;
            playerRef.onTakeDamageFunction = null;
        }
        pumpkinWard1.SetActive(totalHits < totalHitsPerWard);
        pumpkinWard2.SetActive(totalHits < totalHitsPerWard * 0.5f);
    }

    public override void UpdateAbilitySettings()
    {
       
        cooldown = abilityData.cooldown;
    }
}
