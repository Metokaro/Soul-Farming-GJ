using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaHealAbilityScript : BaseAbilityScript
{
    public float healAmount = 40;

    public override void OnAbilityCast()
    {
        Debug.Log("Mega Heal");
    }

    public override void UpdateAbilitySettings()
    {

        
    }
    public MegaHealAbilityScript(string _abilityName) 
    {
        abilityName = _abilityName;

    }
}
