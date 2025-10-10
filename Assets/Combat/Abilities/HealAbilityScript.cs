using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbilityScript : BaseAbilityScript
{
    public float healAmountPerTick = 5;
    public float duration = 5;

    public override void OnAbilityCast()
    {
        Debug.Log("Heal");
    }

    public override void UpdateAbilitySettings()
    {

        Debug.Log(healAmountPerTick + " : " + duration);
        healAmountPerTick += 1;
        duration += 1;
        Debug.Log(healAmountPerTick + " : " + duration);
    }
    public HealAbilityScript(string _abilityName) 
    {
        abilityName = _abilityName;

    }
}
