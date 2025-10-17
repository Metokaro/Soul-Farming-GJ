using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineScript : InteractableObject
{
    public GameObject machineInteractionUI;
    [HideInInspector] public float lifeEnergy;
    [HideInInspector] public RoomGenerator roomGenerator;

    public override void OnInteract()
    {
        machineInteractionUI.SetActive(!machineInteractionUI.activeSelf);
    }

    public override void OnExitRange()
    {
        base.OnExitRange();
        machineInteractionUI.SetActive(false);
    }

    public void UpdateTotalLifeEnergy(float lifeEnergyInput)
    {
        foreach(GameObject machine in (roomGenerator.currentRoom_Data as RoomGenerator.MachineRoomData).machinesInRoom)
        {
            machine.GetComponent<MachineScript>().lifeEnergy = lifeEnergyInput;
        }
    }
}