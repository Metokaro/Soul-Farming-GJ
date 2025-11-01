using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingStationScript : MachineScript
{
    public float healthPerLifeEnergy;
    HealthStationUI uiScript;
    float output;
    float input;
    float maxOutput;
    public override void OnInteract()
    {
        base.OnInteract();
        uiScript = machineInteractionUI.GetComponent<HealthStationUI>();
        maxOutput = CalculateMaxManaOutput();
        UpdateDisplay();
    }
    public void OnSliderChanged()
    {
        input = Mathf.Floor(uiScript.conversionSlider.value * maxOutput *( 1 / healthPerLifeEnergy));
        output = input * healthPerLifeEnergy;
        UpdateDisplay();
    }
    public void Heal()
    {
        lifeEnergy -= input;
        playerRef.health += output;
        input = 0;
        output = 0;
        uiScript.conversionSlider.value = 0;
        maxOutput = CalculateMaxManaOutput();
        UpdateDisplay();
        playerRef.playerScreenRef.UpdateHealthBar();
        UpdateTotalLifeEnergy(lifeEnergy);
    }
    public void OnInputFieldChanged()
    {
        bool isNumber = float.TryParse(uiScript.lifeEnergyInputDisplay.text, out input);
        input = isNumber ? input : 0;
        if (input > lifeEnergy && input > 0)
        {
            input = 0;
            output = 0;
            uiScript.conversionSlider.value = 0;
        }
        else
        {
            output = healthPerLifeEnergy * input;
            uiScript.conversionSlider.value = output / maxOutput;
        }


        UpdateDisplay();
    }

    public float CalculateMaxManaOutput()
    {
        float _maxOutput = 0;
        int r = 1;

        for (int i = 0; i < r; i++)
        {
            if ((i * healthPerLifeEnergy) + playerRef.health >= playerRef.maxHealth)
            {
                _maxOutput = (i * healthPerLifeEnergy);
                if (_maxOutput > lifeEnergy * healthPerLifeEnergy)
                {
                    _maxOutput = lifeEnergy * healthPerLifeEnergy;
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
        uiScript.healthAmountDisplay.text = "Health: " + playerRef.health.ToString();
        uiScript.lifeEnergyAmountDisplay.text = "Life Energy: " + lifeEnergy.ToString();
        uiScript.lifeEnergyInputDisplay.text = input.ToString();
        uiScript.healthOutputDisplay.text = output.ToString();
    }
}
