using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilitiesHandler : MonoBehaviour 
{
    public List<BaseAbilityScript> availableAbilityScripts = new List<BaseAbilityScript>() { new HealAbilityScript("Heal"), new MegaHealAbilityScript("MegaHeal") };
    public List<KeyCode> AbilityKeybinds = new List<KeyCode>();
    public AbilityHotbarDisplay abilityHotbarRef;


    public class Ability
    {
        public int hotbarSlot;
        public KeyCode keybind;
        public BaseAbilityScript abilityScript;
        public AbilityDataTemplate abilityData;
    }
    public HashSet<Ability> abilities = new();

    public void BindAbility(int hotbarSlot, KeyCode keybind, AbilityDataTemplate abilityData)
    {
        if(abilities.Select((x) => x.abilityData).Contains(abilityData))
        {
            return;
        }
        Debug.Log(availableAbilityScripts.Count);
        Ability ability = new()
        {
            hotbarSlot = hotbarSlot,
            keybind = keybind,
            abilityData = abilityData
        };
        try
        {
            ability.abilityScript = availableAbilityScripts.First((x) => x.abilityName == abilityData.abilityName);
        }
        catch
        {
            Debug.Log(abilityData.abilityName + ": has no attached script");
        }
        abilities.Add(ability);
    }

    public void InitializeUnlockedAbilities(List<AbilityDataTemplate> unlockedAbilities)
    {
        foreach (AbilityDataTemplate data in unlockedAbilities)
        {
            BindAbility(abilities.Count + 1, AbilityKeybinds[abilities.Count], data);
        }
        abilityHotbarRef.ReorganizeSlots(abilities.ToList());
    }
}
