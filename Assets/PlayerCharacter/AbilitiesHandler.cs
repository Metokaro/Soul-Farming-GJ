using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilitiesHandler : MonoBehaviour 
{
    public List<BaseAbilityScript> availableAbilityScripts;
    public List<KeyCode> AbilityKeybinds = new List<KeyCode>();
    public AbilityHotbarDisplay abilityHotbarRef;
    public List<GameObject> abilityObjects;

    public class Ability
    {
        public int hotbarSlotIndex;
        public KeyCode keybind;
        public BaseAbilityScript abilityScript;
        public AbilityDataTemplate abilityData;
    }
    public HashSet<Ability> abilities = new();
    public void Awake()
    {
        availableAbilityScripts = new List<BaseAbilityScript>() {
            new HealAbilityScript("Heal", this),
            new MegaHealAbilityScript("MegaHeal", this),
            new DashScript("Dash",this)
            
        
        };
    }
    public void BindAbility(int hotbarSlot, KeyCode keybind, AbilityDataTemplate abilityData)
    {
        if(abilities.Select((x) => x.abilityData).Contains(abilityData))
        {
            return;
        }
        Ability ability = new()
        {
            hotbarSlotIndex = hotbarSlot,
            keybind = keybind,
            abilityData = abilityData
        };
        try
        {
            ability.abilityScript = availableAbilityScripts.First((x) => x.abilityName == abilityData.abilityName);
            ability.abilityScript.abilityData = abilityData;
            ability.abilityScript.slotObjRef = abilityHotbarRef.abilitySlotObjects[ability.hotbarSlotIndex - 1];
            ability.abilityScript.UpdateAbilitySettings();
        }
        catch
        {
            Debug.Log(abilityData.abilityName + ": has no attached script");
        }
        abilities.Add(ability);
    }

    public void InitializeUnlockedAbilities(List<AbilityDataTemplate> unlockedAbilities)
    {
        abilities.Clear();
        unlockedAbilities.ForEach((data) => { BindAbility(abilities.Count + 1, AbilityKeybinds[abilities.Count], data); });
        abilityHotbarRef.SetAbilitySlotData();
        abilityHotbarRef.ReorganizeSlots(abilities.ToList());
    }
}
