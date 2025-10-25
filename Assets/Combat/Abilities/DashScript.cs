using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DashScript : BaseAbilityScript
{
    public GameObject dashFX;
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
        dashFX = abilityHandler.abilityObjects.FirstOrDefault((x) => x.name == "DashEffect");
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
            dashFX.SetActive(true);
            
            yield return new WaitForSeconds(0.31f);
            
            dashFX.SetActive(false);
        }

        IEnumerator ActivateDash()
        {
            playerRef.rb.AddForce(playerRef.directionOrigin.transform.right * 100f, ForceMode2D.Impulse);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("PlayerCharacter"), LayerMask.NameToLayer("Enemy"), true);
            playerRef.canMove = false;
            yield return new WaitForSeconds(0.05f);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("PlayerCharacter"), LayerMask.NameToLayer("Enemy"), false);
            playerRef.canMove = true;
            playerRef.rb.velocity = new();
        }
    }
    public override void UpdateAbilitySettings()
    {
        cooldown = abilityData.cooldown;
    }
}
