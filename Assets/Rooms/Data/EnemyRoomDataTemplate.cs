using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Room Data/Enemy Room", fileName = "new enemy room")]
public class EnemyRoomDataTemplate : ScriptableObject
{
    public enum RoomTiers
    {
        Tier1,
        Tier2,
        Tier3
    }
    public RoomTiers roomTier;
    public enum TilemapTypes
    {
        Floor,
        Walls,
        Props,
        Obstacles,
    }

    [System.Serializable]
    public class TilemapCopyCoordinates
    {
        public TilemapTypes targetTilemap;
        public int minX, maxX;
        public int minY, maxY;
        public int offsetOrigin_x, offsetOrigin_y;
    }

    public List<TilemapCopyCoordinates> copyCoordinates;

    
}
