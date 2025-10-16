using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RoomPicker : MonoBehaviour
{
    RoomGenerator roomGeneratorRef;
    public enum RoomType
    {
        Enemy,
        Machine
    }

    [HideInInspector]public RoomType currentRoomType;

    public ClearedEnemyRooms tier1ClearRequirement;
    public ClearedEnemyRooms tier2ClearRequirement;
    public ClearedEnemyRooms tier3ClearRequirement;
    [System.Serializable]
    public struct ClearedEnemyRooms
    {
        public int tier1;
        public int tier2;
        public int tier3;
    }
   [HideInInspector] public ClearedEnemyRooms clearedEnemyRooms = new();
    // Start is called before the first frame update
    void Start()
    {
        roomGeneratorRef =  GetComponent<RoomGenerator>();
        currentRoomType = RoomType.Enemy;
       roomGeneratorRef.loadableEnemyRoomLayouts=  SetLoadableEnemyLayouts(roomGeneratorRef.enemyRoomLayoutOptions);
        PickCurrentRoom();
    }

    public List<EnemyRoomTemplate> SetLoadableEnemyLayouts(List<EnemyRoomTemplate> enemyRoomLayoutOptions)
    {
        HashSet<EnemyRoomTemplate> loadableLayouts = new();
        if (CheckForRoomClearReq(tier1ClearRequirement))
        {
            Debug.Log("Can load tier 1 enemy rooms");
            loadableLayouts.AddRange(enemyRoomLayoutOptions.Where((x) => x.roomTier == EnemyRoomTemplate.RoomTiers.Tier1));
        }
        if (CheckForRoomClearReq(tier2ClearRequirement))
        {
            Debug.Log("Can load tier 2 enemy rooms");
            loadableLayouts.AddRange(enemyRoomLayoutOptions.Where((x) => x.roomTier == EnemyRoomTemplate.RoomTiers.Tier2));
        }
        if (CheckForRoomClearReq(tier3ClearRequirement))
        {
            Debug.Log("Can load tier 3 enemy rooms");
            loadableLayouts.AddRange(enemyRoomLayoutOptions.Where((x) => x.roomTier == EnemyRoomTemplate.RoomTiers.Tier3));
        }
        return loadableLayouts.ToList();
    }

    public bool CheckForRoomClearReq(ClearedEnemyRooms requirement)
    {
        if( clearedEnemyRooms.tier1 < requirement.tier1)
        {
            return false;
        }
        if (clearedEnemyRooms.tier2 < requirement.tier2)
        {
            return false;
        }
        if (clearedEnemyRooms.tier2 < requirement.tier2)
        {
            return false;
        }
        return true;
    }

    public void IncreaseRoomsClearedCount(EnemyRoomTemplate enemyRoom)
    {
        if(enemyRoom.roomTier == EnemyRoomTemplate.RoomTiers.Tier1)
        {
            clearedEnemyRooms.tier1++;
        }
        if (enemyRoom.roomTier == EnemyRoomTemplate.RoomTiers.Tier2)
        {
            clearedEnemyRooms.tier2++;
        }
        if (enemyRoom.roomTier == EnemyRoomTemplate.RoomTiers.Tier3)
        {
            clearedEnemyRooms.tier3++;
        }
        roomGeneratorRef.loadableEnemyRoomLayouts = SetLoadableEnemyLayouts(roomGeneratorRef.enemyRoomLayoutOptions);
    }

    public void PickCurrentRoom()
    {
        List<RoomDataTemplate> layoutsList = (currentRoomType == RoomType.Enemy) ? roomGeneratorRef.loadableEnemyRoomLayouts.Cast<RoomDataTemplate>().ToList() : roomGeneratorRef.machineRoomLayoutOptions.Cast<RoomDataTemplate>().ToList();
        int randomNumber = UnityEngine.Random.Range(0, layoutsList.Count);
        Debug.Log(layoutsList.Count);
        roomGeneratorRef.currentRoom = layoutsList[randomNumber];
        roomGeneratorRef.GenerateRoom();
    }
}
