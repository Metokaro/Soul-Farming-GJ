using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScreenScript : MonoBehaviour
{
    public TextMeshProUGUI powerDisplay, atkSpeedDisplay, maxHealthDisplay, defenseDisplay, moveSpeedDisplay;
    public TextMeshProUGUI manaDisplay;
    public TextMeshProUGUI healthDisplay;
    public Slider healthBar, manaBar;
    public PlayerController playerRef;

    public TextMeshProUGUI soulsDisplay;
    public TextMeshProUGUI lifeEnergyDisplay;
    public TextMeshProUGUI playerLivesDisplay;
    public GameObject winScreen;
    public TextMeshProUGUI enemyCountDisplay;
    // Start is called before the first frame update
    void Awake()
    {
       playerRef.playerScreenRef = this;
    }

    public void UpdateCurrencies(float souls, float lifeEnergy, bool showLifeEnergy)
    {
        lifeEnergyDisplay.gameObject.SetActive(showLifeEnergy);
        lifeEnergyDisplay.text = "Life Energy: " + lifeEnergy;
        soulsDisplay.text = "Souls: " + souls;
    }

    public void UpdateEnemyCount(int enemies, bool visibility)
    {
        enemyCountDisplay.gameObject.SetActive(visibility);
        enemyCountDisplay.text = "Enemies in room: " + enemies + "<br> (Clear room to proceed)";
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

    public void WinScreen()
    {
        if(playerRef.souls >= 750)
        {
            QuickWait();
        }
    }

    IEnumerator QuickWait()
    {
        winScreen.SetActive(true);
        playerRef.transform.position = new(999, 999, 0);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
