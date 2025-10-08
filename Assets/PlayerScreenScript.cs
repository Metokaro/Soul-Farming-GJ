using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScreenScript : MonoBehaviour
{
    public TextMeshProUGUI powerDisplay, atkSpeedDisplay, maxHealthDisplay, defenseDisplay, moveSpeedDisplay;
    public PlayerController playerRef;
    // Start is called before the first frame update
    void Start()
    {
       
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
