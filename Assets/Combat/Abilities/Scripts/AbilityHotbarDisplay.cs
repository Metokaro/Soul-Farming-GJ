using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityHotbarDisplay : MonoBehaviour
{
    public List<GameObject> abilitySlotObjects;
    public HashSet<AbilitySlotData> abilitySlotDataList = new();
    public GameObject abilityInfoDisplay;
    public TextMeshProUGUI abilityDescription;
    public TextMeshProUGUI abilityName;
    public TextMeshProUGUI manaCost;
  
    public class AbilitySlotData
    {
        public int slotNumber;
        public TextMeshProUGUI keybindDisplay;
        public Image iconDisplay;
        public Slider cooldownBar;
        public AbilitiesHandler.Ability ability;
        public bool slotOccupied = false;
        public AbilitySlotScript slotScript;
        public void UpdateDisplay()
        {
            iconDisplay.gameObject.SetActive(true);
            keybindDisplay.text = ConvertKeyCodeToString(ability.keybind);
            iconDisplay.sprite = ability.abilityData.abilityIcon;
        }
        public void HideDisplay()
        {
            keybindDisplay.text = slotNumber.ToString();
            iconDisplay.gameObject.SetActive(false);
            cooldownBar.gameObject.SetActive(false);
            slotScript.ability = null;
        }
        public string ConvertKeyCodeToString(KeyCode input)
        {
            if((int)input >= 48 && (int)input <= 57)
            {
                return ((int)input - 48).ToString();
            }
            else
            {
                return input.ToString();
            }
           
        }
    }

    public void UpdateAbilityInfoDisplay(bool visible, AbilityDataTemplate ability)
    {
        abilityInfoDisplay.SetActive(visible);
        if(visible == false)
        {
            return;
        }
        abilityDescription.text = ability.abilityDescription;
        manaCost.text = "Mana cost: " + ability.manaCost;
        abilityName.text = "Ability: " + ability.abilityDisplayName;
    }

    public void SetAbilitySlotData()
    {
        abilitySlotDataList.Clear();
        foreach(GameObject slot in abilitySlotObjects)
        {
            AbilitySlotData slotData = new();
            slotData.slotNumber =  abilitySlotObjects.IndexOf(slot) + 1;
            try
            {
                slotData.keybindDisplay = slot.transform.Find("Keybind").GetComponent<TextMeshProUGUI>();
                slotData.iconDisplay = slot.transform.Find("Icon").GetComponent<Image>();
                slotData.cooldownBar = slot.transform.Find("CooldownBar").GetComponent<Slider>();
                slotData.slotScript = slot.GetComponent<AbilitySlotScript>();
            }
            catch
            {
                Debug.Log("could not find slot elements");
            }
            abilitySlotDataList.Add(slotData);
        }
    }
    public void ReorganizeSlots(List<AbilitiesHandler.Ability> abilities)
    {
        
        for (int i = 0; i < abilitySlotDataList.Count; i++)
        {
            AbilitySlotData slot = abilitySlotDataList.ToList()[i];
            slot.slotOccupied = false;
            slot.ability = null;
            
            slot.HideDisplay();
        }

        for (int a = 0; a < abilities.Count; a++)
        {
            AbilitySlotData emptySlot = abilitySlotDataList.FirstOrDefault((x) => x.slotOccupied == false);
            if (emptySlot == null)
            {
                break;
            }
            emptySlot.ability = abilities[a];
            emptySlot.slotScript.ability = emptySlot.ability.abilityData;
            emptySlot.UpdateDisplay();
            emptySlot.slotOccupied = true;
        }
        for (int i = 0; i < abilitySlotDataList.Count; i++)
        {
            AbilitySlotData slot = abilitySlotDataList.ToList()[i];
            if (slot.slotOccupied)
            {
                continue;
            }
            slot.HideDisplay();
        }
    }
}
