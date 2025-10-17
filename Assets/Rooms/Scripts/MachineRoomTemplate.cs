using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Rooms/Machine Room", fileName = "New Machine Room")]
public class MachineRoomTemplate : RoomDataTemplate
{
    public int minStartingLifeEnergy, maxStartingLifeEnergy;
    [System.Serializable]
    public class MachineCoordinates
    {
        public Vector3 location;
        public GameObject machinePrefab;
    }

    public List<MachineCoordinates> machineCoordinates;
}
