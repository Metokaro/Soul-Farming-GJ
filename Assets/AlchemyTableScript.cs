using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AlchemyTableScript : MachineScript
{

    public HashSet<PlayerStats.PlayerStatsTypes> availableStatsToUpgrade = new();
    public HashSet<PlayerStats.PlayerStatsTypes> selectedStats = new();
    AlchemyTable_UI AlchemyTable_UI;
  public  int costPerStat;
    int totalLifeEnergyCost;
    public override void OnInteract()
    {
        base.OnInteract();
        UpdateDisplay();
       
        
    }
    public override void Start()
    {
        base.Start();
        AlchemyTable_UI = machineInteractionUI.GetComponent<AlchemyTable_UI>();
        RandomizeStats();
    }

    public void RandomizeStats()
    {
        int randomAmount = UnityEngine.Random.Range(1, 3);
        List<PlayerStats.PlayerStatsTypes> referenceStats = new() { PlayerStats.PlayerStatsTypes.AtkSpeed, PlayerStats.PlayerStatsTypes.Power, PlayerStats.PlayerStatsTypes.Defense, PlayerStats.PlayerStatsTypes.MaxHealth, PlayerStats.PlayerStatsTypes.MoveSpeed };
        for(int i = 0; i < randomAmount; i++)
        {
            PlayerStats.PlayerStatsTypes randomStat = referenceStats[UnityEngine.Random.Range(0, referenceStats.Count)];
            if(availableStatsToUpgrade.Contains(randomStat))
                {
                return;
            }
            availableStatsToUpgrade.Add(referenceStats[UnityEngine.Random.Range(0, referenceStats.Count)]);
        }
        AlchemyTable_UI.statUIs.ForEach((x) => HideStatUI(x));
        AlchemyTable_UI.statUIs.Where((x) => availableStatsToUpgrade.Contains(x.stat)).ToList().ForEach((x) => ShowStatUI(x));

    }
    
    public void ShowStatUI(AlchemyTable_UI.AlchemyTable_StatUIs statUI)
    {
        GameObject statUIObj = statUI.UI_Obj;
        statUIObj.transform.Find("SelectButton").gameObject.SetActive(true);
        statUIObj.GetComponent<Image>().color = Color.white;
    }

    public void HideStatUI(AlchemyTable_UI.AlchemyTable_StatUIs statUI)
    {
        GameObject statUIObj = statUI.UI_Obj;
        statUIObj.GetComponent<Image>().color = Color.red;
        statUIObj.transform.Find("SelectButton").gameObject.SetActive(false);
    }
    public void SelectStat(string stat)
    {
        var selectedStat = AlchemyTable_UI.statUIs.FirstOrDefault((x) => x.stat.ToString() == stat);
        if(selectedStat.isSelected == false)
        {
            selectedStat.UI_Obj.transform.Find("SelectButton").GetComponent<Image>().color = Color.green;
            selectedStat.isSelected = true;
            totalLifeEnergyCost += costPerStat;
            selectedStats.Add(selectedStat.stat);
        }
        else
        {
            selectedStat.UI_Obj.transform.Find("SelectButton").GetComponent<Image>().color = Color.grey;
            selectedStat.isSelected = false;
            totalLifeEnergyCost -= costPerStat;
            selectedStats.Remove(selectedStat.stat);
        }
        UpdateDisplay();

    }

    public void UpgradeStats()
    {
        foreach(var stat in selectedStats)
        {
            playerRef.playerStats.IncreaseStats(stat, 1);
        }
        if(lifeEnergy >= totalLifeEnergyCost)
        {
             lifeEnergy -= totalLifeEnergyCost;
            UpdateDisplay();
            UpdateTotalLifeEnergy(lifeEnergy);
            
        }
    }

    public void UpdateDisplay()
    {
        AlchemyTable_UI.lifeEnergyDisplay.text = "Life Energy: "+lifeEnergy;

        if(lifeEnergy >= totalLifeEnergyCost)
        {
            AlchemyTable_UI.lifeEnergyCostDisplay.color = new Color(0.7295287f, 1, 0.2028302f,1);
        }
        else
        {
            AlchemyTable_UI.lifeEnergyCostDisplay.color = Color.red;
        }
        AlchemyTable_UI.lifeEnergyCostDisplay.text = "total cost: " + totalLifeEnergyCost;
    }
}
