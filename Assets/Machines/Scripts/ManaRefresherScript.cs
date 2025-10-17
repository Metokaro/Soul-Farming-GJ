using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaRefresherScript : MachineScript
{
    public float manaPerLifeEnergy;
    ManaRefresherUI uiScript;
    PlayerController playerRef;
    float output;
    float input;
    float maxOutput;
    public override void OnInteract()
    {
        base.OnInteract();
        uiScript = machineInteractionUI.GetComponent<ManaRefresherUI>();
        playerRef = FindObjectOfType<PlayerController>();
        maxOutput = CalculateMaxManaOutput();
        UpdateDisplay();
    }
    public void OnSliderChanged()
    {
        input = Mathf.Floor(uiScript.conversionSlider.value * maxOutput / manaPerLifeEnergy);
        output = input * manaPerLifeEnergy;
        UpdateDisplay();
    }
    public void RefreshMana()
    {
        lifeEnergy -= input;
        playerRef.mana += output;
        input = 0;
        output = 0; 
        uiScript.conversionSlider.value = 0;
        maxOutput = CalculateMaxManaOutput();
        UpdateDisplay();
        
        UpdateTotalLifeEnergy(lifeEnergy);
    }
    public void OnInputFieldChanged()
    {
        bool isNumber = float.TryParse(uiScript.lifeEnergyAmountInput.text, out input);
        input = isNumber ? input : 0;
        if(input > lifeEnergy && input > 0)
        {
            input= 0;
            output = 0;
            uiScript.conversionSlider.value = 0;
        }
        else
        {
            output = manaPerLifeEnergy * input;
            uiScript.conversionSlider.value = output / maxOutput;
        }

        
        UpdateDisplay();
    }

    public float CalculateMaxManaOutput()
    {
        float _maxOutput = 0;
        int r = 1;
 
        for(int i = 0; i < r; i++)
        {
            if((i * manaPerLifeEnergy) + playerRef.mana >= playerRef.maxMana)
            {
                _maxOutput = (i * manaPerLifeEnergy);
                if(_maxOutput > lifeEnergy * manaPerLifeEnergy)
                {
                    _maxOutput = lifeEnergy * manaPerLifeEnergy;
                }
                break;
               
            }
            else
            {
                r++;
            }
            
        }
        
        return _maxOutput;

    }

    public void UpdateDisplay()
    {
        uiScript.manaAmountDisplay.text = "Mana: "+playerRef.mana.ToString();
        uiScript.lifeEnergyDisplay.text = "Life Energy: " + lifeEnergy.ToString();
        uiScript.lifeEnergyAmountInput.text = input.ToString();
        uiScript.manaOutputDisplay.text = output.ToString();
    }
}
