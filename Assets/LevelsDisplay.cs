using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelsDisplay : MonoBehaviour
{
    public Slider levelsBar;
    public TextMeshProUGUI levelsAmount;
    public TextMeshProUGUI expAmount;
    public PlayerController playerRef;
    void Start()
    {
        playerRef.playerLevellingSystem.displayRef = this;
        UpdateDisplay();
    }
    
    public void UpdateDisplay()
    {
        levelsAmount.text = "Level " + playerRef.playerLevellingSystem.level;
        expAmount.text = playerRef.playerLevellingSystem.exp + "/" + playerRef.playerLevellingSystem.maxExp;
        levelsBar.value = playerRef.playerLevellingSystem.exp/ playerRef.playerLevellingSystem.maxExp;
    }
}
