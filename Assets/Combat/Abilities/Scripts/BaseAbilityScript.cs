using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbilityScript
{
    public string abilityName;
    public AbilitiesHandler abilityHandler;
    public float cooldown;
    public bool cooldownActive;
    public AbilityDataTemplate abilityData;
    public PlayerController playerRef;
    public GameObject slotObjRef;

    public void UnableToCast()
    {
      GameObject popup =  GameObject.Instantiate(slotObjRef.GetComponentInParent<AbilityHotbarDisplay>().noManaPopup, slotObjRef.transform.parent.position, Quaternion.identity);
        popup.SetActive(true);
        popup.transform.SetParent(slotObjRef.transform.parent.parent);
    }
    public void DepleteMana()
    {
        if(cooldownActive)
        {
            return;
        }
        playerRef.mana -= abilityData.manaCost;
        playerRef. playerScreenRef.UpdateManaBar();
    }
    public abstract void OnAbilityCast();
    public virtual void OnAbilityEnd() { }
    public virtual void UpdateAbilitySettings() { }

    public BaseAbilityScript (string _abilityName, AbilitiesHandler _abilitiesHandler)
    { 
        abilityName = _abilityName;
        abilityHandler = _abilitiesHandler;
        playerRef = abilityHandler.transform.GetComponent<PlayerController>();
    }
}
