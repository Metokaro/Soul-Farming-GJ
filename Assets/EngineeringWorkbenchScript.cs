using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EngineeringWorkbenchScript : MachineScript
{
     EngineeringWorkbench_UI machineUIScript;
     BaseWeaponScript currentWeapon;
    AbilityDataTemplate nextAbility;


    public float upgradeCost;
    public override void OnInteract()
    {
        base.OnInteract();
        currentWeapon = playerRef.equipSystem.currentWeaponObj.GetComponent<BaseWeaponScript>();
        nextAbility = GetNextAbility();
        UpdateDisplay();
    }

    public void UpgradeWeapon()
    {
        if(lifeEnergy >= upgradeCost)
        {
            lifeEnergy -= upgradeCost;
            currentWeapon.weaponLevel++;
            playerRef.abilitiesHandler.InitializeUnlockedAbilities(currentWeapon.UnlockAbilities().ToList());
            UpdateTotalLifeEnergy(lifeEnergy);
            UpdateCost();
        }


        nextAbility = GetNextAbility();
        UpdateDisplay();
    }

    void UpdateCost()
    {
        upgradeCost += upgradeCost * ((currentWeapon.weaponLevel - 1) * 0.5f);
    }

    public void UpdateDisplay()
    {
        float maxLevel = currentWeapon.weaponData.unlockedAbilities.OrderByDescending((x) => x.levelReq).First().levelReq;
        bool maxLevelReached = currentWeapon.weaponLevel >= maxLevel;
        machineUIScript.maxLevelPopup.gameObject.SetActive(maxLevelReached);
        machineUIScript.upgradeButton.gameObject.SetActive(!maxLevelReached);
        machineUIScript.abilityIcon.sprite = nextAbility.abilityIcon;
        machineUIScript.abilityName.text = nextAbility.abilityName;
        machineUIScript.currentLevel.text = "Level: "+ currentWeapon.weaponLevel;
        machineUIScript.nextLevel.text = "Level: "+ (currentWeapon.weaponLevel + 1);
        machineUIScript.lifeEnergyCost.text = "Costs: " + upgradeCost + " Life Energy";
        machineUIScript.lifeEnergyAmount.text = "Life Energy: " + lifeEnergy;
        machineUIScript.weaponIcon.sprite = currentWeapon.GetComponent<SpriteRenderer>().sprite;
    }

    public AbilityDataTemplate GetNextAbility()
    {
        return currentWeapon.weaponData.unlockedAbilities.FirstOrDefault((x) => x.levelReq == currentWeapon.weaponLevel + 1).ability;
    }

    public override void Start()
    {
        base.Start();
        machineUIScript = machineInteractionUI.GetComponent<EngineeringWorkbench_UI>();
    }
}
