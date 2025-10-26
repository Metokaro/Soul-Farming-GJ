using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeathAuraScript : BaseAbilityScript
{
    public float AOE_radius = 3f;
    public float AOE_damage = 50f;
    public GameObject deathAuraEffect;
    
    public DeathAuraScript(string _abilityName, AbilitiesHandler _abilitiesHandler) : base(_abilityName, _abilitiesHandler)
    {

    }

    public override void OnAbilityCast()
    {
        if(cooldownActive)
        {
            return;
        }
        deathAuraEffect = abilityHandler.abilityObjects.FirstOrDefault((x) => x.name == "DeathAuraEffect");
        abilityHandler.StartCoroutine(Cooldown());
        abilityHandler.StartCoroutine(DeathAuraEffect());
        CreateAOEAttack();
        
        IEnumerator Cooldown()
        {
            cooldownActive = true; slotObjRef.GetComponent<AbilitySlotScript>().ShowCooldownBar(cooldown);
            yield return new WaitForSeconds(cooldown);
            cooldownActive = false; slotObjRef.GetComponent<AbilitySlotScript>().HideCooldownBar();
        }
        IEnumerator DeathAuraEffect()
        {
            deathAuraEffect.SetActive(true);
            yield return new WaitForSeconds(0.43f);
            deathAuraEffect.SetActive(false);
        }
    }

    public void CreateAOEAttack()
    {
        List<RaycastHit2D> hits = Physics2D.CircleCastAll(playerRef.transform.position, AOE_radius, Vector2.zero,1, playerRef.targetableLayerMask).ToList();

        hits.ForEach(hit => { hit.collider.transform.parent.GetComponent<BaseAIBehaviour>().TakeDamage(AOE_damage); });
    }
    public override void UpdateAbilitySettings()
    {
        cooldown = abilityData.cooldown;
    }
}
