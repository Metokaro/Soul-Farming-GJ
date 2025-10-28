using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FrenzyScript : BaseAbilityScript
{
    public float duration = 3f;
    public GameObject fx;
    public FrenzyScript(string _abilityName, AbilitiesHandler _abilitiesHandler) : base(_abilityName, _abilitiesHandler)
    {
    }

    public override void OnAbilityCast()
    {
        if (cooldownActive)
        {
            return;
        }

        abilityHandler.StartCoroutine(Cooldown());
        abilityHandler.StartCoroutine(Duration());
        FrenzyEffect();
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
            playerRef.equipSystem.currentWeaponObj.GetComponent<BaseWeaponScript>().calculatedWeaponSpeed *= 0.5f;
            yield return new WaitForSeconds(duration);
            GameObject.Destroy(fx);
            playerRef.equipSystem.currentWeaponObj.GetComponent<BaseWeaponScript>().calculatedWeaponSpeed *= 2f;

        }


        
    }

    void FrenzyEffect()
    {
        fx = GameObject.Instantiate(abilityHandler.abilityObjects.FirstOrDefault((x) => x.name == "DeathAuraEffect"), playerRef.equipSystem.currentWeaponObj.transform.position, Quaternion.identity);
        fx.transform.localScale = Vector2.one * 0.5f;
        fx.SetActive(true);
        fx.transform.SetParent(playerRef.equipSystem.currentWeaponObj.transform);
    }

    public override void UpdateAbilitySettings()
    {
        cooldown = abilityData.cooldown;
    }
}
