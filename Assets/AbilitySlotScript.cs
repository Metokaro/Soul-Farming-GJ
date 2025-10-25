using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySlotScript : MonoBehaviour
{
    public Slider cooldownBar;
    float cooldownBarValue;
    float cooldownDuration;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void ShowCooldownBar(float _cooldownDuration)
    {
        cooldownBar.gameObject.SetActive(true);
        cooldownDuration = _cooldownDuration;
    }

    public void HideCooldownBar()
    {
        cooldownBar.gameObject.SetActive(false);
        cooldownBarValue = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if(cooldownBar.gameObject.activeSelf)
        {
            cooldownBarValue += Time.deltaTime;
            cooldownBar.value = cooldownBarValue / cooldownDuration;
        }
    }
}
