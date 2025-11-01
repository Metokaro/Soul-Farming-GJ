using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DashScript : BaseAbilityScript
{
    public GameObject dashTrailRenderer;
    public GameObject dashHitbox;
    public float damage = 35;
    public DashScript(string _abilityName, AbilitiesHandler _abilitiesHandler) : base(_abilityName, _abilitiesHandler)
    {
    }

    public override void OnAbilityCast()
    {
        if(cooldownActive)
        {
            return;
        }

        abilityHandler.StartCoroutine(Cooldown());
        abilityHandler.StartCoroutine(ActivateEffect());  
        abilityHandler.StartCoroutine(ActivateDash());
        

        IEnumerator Cooldown()
        {
            cooldownActive = true; slotObjRef.GetComponent<AbilitySlotScript>().ShowCooldownBar(cooldown);
            yield return new WaitForSeconds(cooldown);
            cooldownActive = false; slotObjRef.GetComponent<AbilitySlotScript>().HideCooldownBar();
        }
        IEnumerator ActivateEffect()
        {
            
            
            yield return new WaitForSeconds(0.31f);
            
           
        }

        IEnumerator ActivateDash()
        {
            if(playerRef.equipSystem.currentWeaponObj.GetComponent<BaseWeaponScript>().hitboxObj)
            {
                GameObject.Destroy(playerRef.equipSystem.currentWeaponObj.GetComponent<BaseWeaponScript>().hitboxObj);
            }
            playerRef.rb.AddForce(playerRef.directionOrigin.transform.right * 100f, ForceMode2D.Impulse);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("PlayerCharacter"), LayerMask.NameToLayer("Enemy"), true);
            playerRef.canMove = false; 
            dashTrailRenderer.SetActive(true);
            dashHitbox.SetActive(true);
            yield return new WaitForSeconds(0.05f);
            
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("PlayerCharacter"), LayerMask.NameToLayer("Enemy"), false);  
            playerRef.rb.velocity = new();
            yield return new WaitForSeconds(0.1f);
            dashHitbox.SetActive(false);
            yield return new WaitForSeconds(0.15f);
            playerRef.canMove = true;
            yield return new WaitForSeconds(0.25f);
            dashTrailRenderer.SetActive(false);
        }
    }
    public override void UpdateAbilitySettings()
    {
        dashTrailRenderer = abilityHandler.abilityObjects.FirstOrDefault((x) => x.name == "DashTrailRenderer");
        dashHitbox = abilityHandler.abilityObjects.FirstOrDefault((x) => x.name == "DashHitbox");
        dashHitbox.GetComponent<DashHitboxScript>().refScript = this;
        cooldown = abilityData.cooldown;
    }
}
