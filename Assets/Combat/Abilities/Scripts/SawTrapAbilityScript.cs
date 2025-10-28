using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SawTrapAbilityScript : BaseAbilityScript
{
    public GameObject sawTrapPrefab;
    public float trapDuration = 7;
    public float stunDuration = 3;
    public SawTrapAbilityScript(string _abilityName, AbilitiesHandler _abilitiesHandler) : base(_abilityName, _abilitiesHandler)
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
            GameObject sawTrapObj = CreateSawTrap();
            playerRef.equipSystem.currentWeaponObj.GetComponent<BaseWeaponScript>().calculatedWeaponSpeed *= 0.5f;
            yield return new WaitForSeconds(trapDuration);
            GameObject.Destroy(sawTrapObj);
            playerRef.equipSystem.currentWeaponObj.GetComponent<BaseWeaponScript>().calculatedWeaponSpeed *= 2f;

        }



    }
    GameObject CreateSawTrap()
    {
        GameObject sawTrapobj = GameObject.Instantiate(sawTrapPrefab, playerRef.transform.position, Quaternion.identity);
        sawTrapobj.SetActive(true);
        sawTrapobj.GetComponent<SawTrapScript>().refScript = this;
        return sawTrapobj;
    }
  public void TriggerTrap(GameObject targetEnemy)
    {
       abilityHandler.StartCoroutine(TriggerSawTrap());
         IEnumerator TriggerSawTrap()
        {
            targetEnemy.GetComponent<BaseAIBehaviour>().Stunned(true);
            yield return new WaitForSeconds(stunDuration);
            targetEnemy.GetComponent<BaseAIBehaviour>().Stunned(false);
        }
    }
   

    public override void UpdateAbilitySettings()
    {
        sawTrapPrefab = abilityHandler.abilityObjects.FirstOrDefault((x) => x.name == "SawTrap");
        cooldown = abilityData.cooldown;
    }
}
