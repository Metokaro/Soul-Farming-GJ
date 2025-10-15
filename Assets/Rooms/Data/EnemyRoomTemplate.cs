using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Rooms/Enemy Room", fileName = "New Enemy Room")]
public class EnemyRoomTemplate : RoomDataTemplate
{
    public enum RoomTiers
    {
        Tier1,
        Tier2,
        Tier3
    }
    public RoomTiers roomTier;
    public int startingMana;
    public int basicEnemiesCount_min, basicEnemiesCount_max;
    public int intermediateEnemiesCount_min, intermediateEnemiesCount_max;
    public int advancedEnemiesCount_min, advancedEnemiesCount_max;
}
