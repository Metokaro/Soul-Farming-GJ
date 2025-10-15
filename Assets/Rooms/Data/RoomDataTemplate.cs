using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomDataTemplate : ScriptableObject
{
    public enum TilemapTypes
    {
        Floor,
        Walls,
        WallColliders,
        Props,
        Obstacles,
    }

    [System.Serializable]
    public class TilemapCopyCoordinates
    {
        public TilemapTypes targetTilemap;
        public int minX, maxX;
        public int minY, maxY;
        public int offsetFromOrigin_x, offsetFromOrigin_y;
    }

    public Vector3 entranceLocation;
    public Vector3 exitLocation;
    public Vector3 roomOrigin;


    public GameObject entranceObj;
    public GameObject exitObj;

    public List<TilemapCopyCoordinates> copyCoordinates;

    public void SetEntranceLocationOffset(out Vector3 offsetLocation)
    {
        float absoluteOffset_x = entranceLocation.x - roomOrigin.x;
        float absoluteOffset_y = entranceLocation.y - roomOrigin.y;

        //float offset_x = (entranceLocation.x < 0) ? absoluteOffset_x * -1 : absoluteOffset_x;
        //float offset_y = (entranceLocation.y < 0) ? absoluteOffset_y * -1 : absoluteOffset_y;

        offsetLocation = new(absoluteOffset_x, absoluteOffset_y);
    }

    public void SetExitLocationOffset(out Vector3 offsetLocation)
    {
        float absoluteOffset_x =  exitLocation.x - roomOrigin.x;
        float absoluteOffset_y =  exitLocation.y - roomOrigin.y;

        //float offset_x = (exitLocation.x < 0) ? absoluteOffset_x * -1 : absoluteOffset_x;
        //float offset_y = (exitLocation.y <0) ? absoluteOffset_y * -1 : absoluteOffset_y;

        offsetLocation = new(absoluteOffset_x, absoluteOffset_y);
    }
}
