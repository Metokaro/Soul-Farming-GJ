using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats
{
    public enum PlayerStatsTypes
    {
        Power,
        AtkSpeed,
        MaxHealth,
        Defense,
        MoveSpeed,
    }

    public float powerMultiplier = 1f;
    public float atkSpeedMultiplier = 1f;
    public float maxHealth = 100f;
    public float defenseMultiplier = 0f;
    public float moveSpeed = 3f;

    public float powerIncrement = 0.1f;
    public float defenseIncrement = 0.2f;
    public float moveSpeedIncrement = 0.1f;
    public float maxHealthIncrement = 10f;
    public float atkSpeedIncrement = 0.05f;

 
    public void IncreaseStats(PlayerStatsTypes targetStats, float incrementLevel)
    {
        switch (targetStats)
        {
            case PlayerStatsTypes.Power:
                powerMultiplier += powerIncrement * incrementLevel;
                break;
            case PlayerStatsTypes.AtkSpeed:
                atkSpeedMultiplier += atkSpeedIncrement * incrementLevel;
                break;
            case PlayerStatsTypes.MaxHealth:
                maxHealth += maxHealthIncrement * incrementLevel;
                break;
            case PlayerStatsTypes.Defense:
                defenseMultiplier += defenseIncrement * incrementLevel;
                break;
            case PlayerStatsTypes.MoveSpeed:
                moveSpeed += moveSpeedIncrement * incrementLevel;
                break;
        }
        
    }

    public void DisplayStats(TextMeshProUGUI powerDisplay, TextMeshProUGUI atkSpeedDisplay, TextMeshProUGUI maxHealthDisplay, TextMeshProUGUI defenseDisplay, TextMeshProUGUI moveSpeedDisplay)
    {
        powerDisplay.text = "Power: "+ powerMultiplier + "x";
        maxHealthDisplay.text = "Max HP: "+maxHealth + "HP";
        atkSpeedDisplay.text = "ATK speed: "+atkSpeedMultiplier + "x";
        defenseDisplay.text = "Defense: " +defenseMultiplier;
        moveSpeedDisplay.text = "Move speed: "+ moveSpeed;
    }
}
