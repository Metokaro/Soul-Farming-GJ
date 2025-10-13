using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomGenerator : MonoBehaviour
{
    public List<EnemyRoomDataTemplate> enemyRoomOptions;
    public List<Tilemap> templateTilemaps;
    public List<Tilemap> originalTilemaps;
    [HideInInspector]public EnemyRoomDataTemplate currentEnemyRoom;

    public struct RoomSize
    {
     public   int roomSize_X;
       public int roomSize_Y;
    }

    public class TileInfo
    {
        public Vector2Int tileLocation;
        public TileBase tileBase;
    }
    public class TilemapInfo
    {
        public List<TileInfo> tileInfoList=new();
        public RoomSize roomSize;
        public EnemyRoomDataTemplate.TilemapCopyCoordinates copyCoordinates;
    }

    public List<TilemapInfo> tilemapInfoList = new();
    public Tilemap GetTargetTilemap(EnemyRoomDataTemplate.TilemapTypes tilemapType, List<Tilemap> tilemapList)
    {
     
        return tilemapList.FirstOrDefault((x) =>LayerMask.LayerToName(x.gameObject.layer) == tilemapType.ToString());
    }

    public List<RoomSize> tilemapSizes = new();
    public void CalculateRoomSizes()
    {       
        foreach(var coordinates in currentEnemyRoom.copyCoordinates)
        {
            tilemapSizes.Add(new RoomSize() { 
                
                roomSize_X = ((coordinates.maxX - coordinates.minX) > 0) ? (coordinates.maxX - coordinates.minX) +1 : ((coordinates.maxX - coordinates.minX) * -1) - 1,
                roomSize_Y = ((coordinates.maxY - coordinates.minY) > 0) ? (coordinates.maxY - coordinates.minY) +1: ((coordinates.maxY - coordinates.minY) * -1)-1
            }    
           
            );
        }
    }

    public void GetTileMapTemplateInfo()
    {
        foreach (var coordinates in currentEnemyRoom.copyCoordinates)
        {
            tilemapInfoList.Add(new() { roomSize = tilemapSizes[currentEnemyRoom.copyCoordinates.IndexOf(coordinates)], copyCoordinates = coordinates });
        }
        foreach (var tilemapInfo in tilemapInfoList)
        {
            for (int x = 0; x < tilemapInfo.roomSize.roomSize_X; x++)
            {
                for (int y = 0; y < tilemapInfo.roomSize.roomSize_Y; y++)
                {
                    TileInfo tileInfo = new()
                    {
                        tileLocation = new(tilemapInfo.copyCoordinates.minX + x, tilemapInfo.copyCoordinates.minY + y)
                    };
                    tileInfo.tileBase = GetTargetTilemap(tilemapInfo.copyCoordinates.targetTilemap, templateTilemaps).GetTile(new(tileInfo.tileLocation.x, tileInfo.tileLocation.y));
                    tilemapInfo.tileInfoList.Add(tileInfo);
                    //Debug.Log(tilemapInfo.copyCoordinates.targetTilemap + " = " + tileInfo.tileLocation + " : " + tileInfo.tileBase);
                }
            }
        }
    }

    public void SetTilemapInfo(Vector3Int startPos)
    {
        foreach(var tilemapInfo in tilemapInfoList)
        {
            for(int x = 0; x < tilemapInfo.roomSize.roomSize_X; x++)
            {
                for (int y = 0; y< tilemapInfo.roomSize.roomSize_Y; y++)
                {
                    int getLocation_x = tilemapInfo.copyCoordinates.minX + x;
                    int getLocation_y = tilemapInfo.copyCoordinates.minY + y;

                    TileInfo tileInfo = tilemapInfo.tileInfoList.FirstOrDefault((tile) => tile.tileLocation.x == getLocation_x && tile.tileLocation.y == getLocation_y);

                    Tilemap currentTilemapRef = GetTargetTilemap(tilemapInfo.copyCoordinates.targetTilemap, originalTilemaps);
                    Vector3Int newTileLocation = new(startPos.x + x + tilemapInfo.copyCoordinates.offsetOrigin_x, startPos.y + y + tilemapInfo.copyCoordinates.offsetOrigin_y, 0);
                    currentTilemapRef.SetTile(newTileLocation, tileInfo.tileBase);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        int randomNumber = Random.Range(0, enemyRoomOptions.Count- 1);
        currentEnemyRoom = enemyRoomOptions[randomNumber];
        CalculateRoomSizes();
        GetTileMapTemplateInfo();
        SetTilemapInfo(new());
    }

    public Vector2Int TilemapPositionSeeker()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        return new((int)mousePos.x, (int)mousePos.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) { Debug.Log(TilemapPositionSeeker()); }
    }
}
