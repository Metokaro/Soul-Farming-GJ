using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class SoulConverterScript : MachineScript
{
    public float lifeEnergyPerSoul;
    SoulConverterUI uiScript;
    float outputAmount;
    float inputAmount;
    public override void Start()
    {
        base.Start();
        lifeEnergy = Random.Range((roomGenerator.currentRoom as MachineRoomTemplate).minStartingLifeEnergy, (roomGenerator.currentRoom as MachineRoomTemplate).maxStartingLifeEnergy + 1);
        UpdateTotalLifeEnergy(lifeEnergy);
    }
    public override void OnInteract()
    {
        base.OnInteract();
       uiScript = machineInteractionUI.GetComponent<SoulConverterUI>();
        UpdateDisplay();

    }
    public void Convert()
    {
        lifeEnergy += outputAmount;
        playerRef.souls -= inputAmount;

        inputAmount = 0;
        outputAmount = 0;
        uiScript.conversionSlider.value = 0;
        UpdateDisplay();
        UpdateTotalLifeEnergy(lifeEnergy);
    }
    public void OnSliderChanged()
    {
         inputAmount = Mathf.Floor((uiScript.conversionSlider.value * playerRef.souls));
         outputAmount = inputAmount * lifeEnergyPerSoul;
        UpdateDisplay();
    }
    public void OnInputFieldChanged()
    {
        bool IsNumber = float.TryParse(uiScript.soulsAmountInput.text, out inputAmount);
        inputAmount = IsNumber ? inputAmount : 0;
        if(inputAmount > playerRef.souls && inputAmount > 0)
        {
            inputAmount = 0;
            outputAmount = 0;
            uiScript.conversionSlider.value = 0;
        }
        else
        {
            uiScript.conversionSlider.value = inputAmount / playerRef.souls;
            outputAmount = lifeEnergyPerSoul * inputAmount;
        }
        
        UpdateDisplay();
       
    }
    void UpdateDisplay()
    {
        uiScript.soulsAmountInput.text = inputAmount.ToString();
        uiScript.lifeEnergyDisplay.text = "Life Energy: " + lifeEnergy;
        uiScript.soulsAmountDisplay.text = "Souls: " + playerRef.souls;
        uiScript.lifeEnergyOutputDisplay.text = outputAmount.ToString();
    }

    
}
