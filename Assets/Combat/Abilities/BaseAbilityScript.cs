using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbilityScript
{
    public string abilityName;
    public abstract void OnAbilityCast();
    public virtual void OnAbilityEnd() { }
    public virtual void UpdateAbilitySettings() { }
}
