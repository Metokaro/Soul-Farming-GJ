using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MachineScript : InteractableObject
{
    public GameObject machineInteractionUI;
    [HideInInspector] public float lifeEnergy;
    [HideInInspector] public RoomGenerator roomGenerator;
    [HideInInspector] public PlayerController playerRef;

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
        (roomGenerator.currentRoom_Data as RoomGenerator.MachineRoomData).machinesInRoom.ToList().ForEach(r => { r.GetComponent<MachineScript>().lifeEnergy = lifeEnergyInput; });
    }
}