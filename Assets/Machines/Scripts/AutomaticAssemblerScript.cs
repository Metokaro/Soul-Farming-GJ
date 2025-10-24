using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutomaticAssemblerScript : MachineScript
{
    public List<WeaponDataTemplate> obtainableWeapons;
    WeaponDataTemplate currentCraftableWeapon;
    AutomaticAssembler_UI automaticAssembler_UI;

    bool weaponCrafted;
    float lifeEnergyCost;
    public override void OnInteract()
    {
        base.OnInteract();
        
        
    }
    public override void Start()
    {
        base.Start();
        automaticAssembler_UI = machineInteractionUI.GetComponent<AutomaticAssembler_UI>();
        SelectRandomCraftableWeapon();
    }
    public void SelectRandomCraftableWeapon()
    {
        currentCraftableWeapon = obtainableWeapons[Random.Range(0, obtainableWeapons.Count)];
        lifeEnergyCost = currentCraftableWeapon.lifeEnergyCost;
        /*currentCraftableWeapon = obtainableWeapons[Random.Range(0, obtainableWeapons.Count)];*/
        UpdateDisplay();
    }
    public void UpdateDisplay()
    {
        automaticAssembler_UI.WeaponImage.sprite = currentCraftableWeapon.weaponPrefab.GetComponent<SpriteRenderer>().sprite;
        automaticAssembler_UI.LifeEnergyCostDisplay.text = "Costs " + lifeEnergyCost + " Life Energy";
        automaticAssembler_UI.LifeEnergyAmountDisplay.text = "Life Energy: " + lifeEnergy;
        if(weaponCrafted)
        {
            automaticAssembler_UI.buildButton.gameObject.SetActive(false);
            automaticAssembler_UI.LifeEnergyCostDisplay.color = Color.red;
            automaticAssembler_UI.LifeEnergyCostDisplay.text = "Weapon Crafted";
        }
    }
    public void CraftWeapon()
    {
        if(lifeEnergy >= lifeEnergyCost && weaponCrafted ==false)
        {
          
            playerRef.equipSystem.EquipNewWeapon(currentCraftableWeapon);
            lifeEnergy -= lifeEnergyCost;
            UpdateTotalLifeEnergy(lifeEnergy);
            weaponCrafted = true;
            UpdateDisplay();
        }
    }
}
