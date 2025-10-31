using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefectSoulConverterScript : InteractableObject
{
    public GameObject ui;
    public override void OnInteract()
    {
        ui.SetActive(true);
    }

    public override void OnExitRange()
    {
        ui.SetActive(false);
    }
}
