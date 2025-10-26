using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScreenScript : MonoBehaviour
{
    public TextMeshProUGUI powerDisplay, atkSpeedDisplay, maxHealthDisplay, defenseDisplay, moveSpeedDisplay;
    public TextMeshProUGUI manaDisplay;
    public TextMeshProUGUI healthDisplay;
    public Slider healthBar, manaBar;
    public PlayerController playerRef;
    // Start is called before the first frame update
    void Awake()
    {
       playerRef.playerScreenRef = this;
    }

    public void UpdateHealthBar()
    {
        
        healthDisplay.text = playerRef.health + " / " + playerRef.maxHealth;
        healthBar.value = playerRef.health/playerRef.maxHealth;
    }

    public void UpdateManaBar()
    {
        manaDisplay.text = playerRef.mana + " / " + playerRef.maxMana;
        manaBar.value = playerRef.mana / playerRef.maxMana;
    }

    public void DisplayButton()
    {
        playerRef.playerStats.DisplayStats(powerDisplay, atkSpeedDisplay, maxHealthDisplay, defenseDisplay, moveSpeedDisplay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
