using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Ability Data", fileName = "New Ability")]
public class AbilityDataTemplate : ScriptableObject
{
    public string abilityName;
    public string abilityDisplayName;
    public Sprite abilityIcon;
    public enum AbilityTypes
    {
        Passive, Active
    }
    public AbilityTypes abilityType;
}
