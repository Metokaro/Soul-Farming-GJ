using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomGenerator : MonoBehaviour
{
    public AstarPath pathfinder;
    public List<EnemyRoomTemplate> enemyRoomLayoutOptions;
    public List<MachineRoomTemplate> machineRoomLayoutOptions;
    [HideInInspector] public List<EnemyRoomTemplate> loadableEnemyRoomLayouts = new();
    public List<Tilemap> templateTilemaps;
    public List<Tilemap> originalTilemaps;
    public PlayerController playerController;
    [HideInInspector]public RoomDataTemplate currentRoom;
    RoomPicker roomPickerRef;
    GameObject exitObjInstance;
    GameObject entranceObjInstance;

    [HideInInspector]public int clearedRoomsCount;
    EnemySpawnHandler enemySpawnHandler;
    

    public class EnemyRoomData : RoomData
    {
        public int basicEnemiesCount;
        public int intermediateEnemiesCount;
        public int advancedEnemiesCount;
        public HashSet<GameObject> enemiesInRoom = new();
        public int startingMana;
    }

    public class MachineRoomData : RoomData
    {
        public int startingLifeEnergy;
        public HashSet<GameObject> machinesInRoom = new();
    }

    public class RoomData
    {
        public int RoomNumber;
    }

    public RoomData currentRoom_Data;

    public struct RoomSize
    {
       public int roomSize_X;
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
        public RoomDataTemplate.TilemapCopyCoordinates copyCoordinates;
    }

    public List<TilemapInfo> tilemapInfoList = new();
    public Tilemap GetTargetTilemap(RoomDataTemplate.TilemapTypes tilemapType, List<Tilemap> tilemapList)
    {
     
        return tilemapList.FirstOrDefault((x) =>LayerMask.LayerToName(x.gameObject.layer) == tilemapType.ToString());
    }

    public List<RoomSize> tilemapSizes = new();
    public void CalculateRoomSizes()
    {       
        foreach(var coordinates in currentRoom.copyCoordinates)
        {
            tilemapSizes.Add(new RoomSize() { 
                
                roomSize_X = ((coordinates.maxX - coordinates.minX) > 0) ? (coordinates.maxX - coordinates.minX): ((coordinates.maxX - coordinates.minX) * -1),
                roomSize_Y = ((coordinates.maxY - coordinates.minY) > 0) ? (coordinates.maxY - coordinates.minY): ((coordinates.maxY - coordinates.minY) * -1)
            }    
           
            );
        }
    }

    public void GetTileMapTemplateInfo()
    {
        //foreach (var coordinates in currentRoom.copyCoordinates)
        //{
        //    tilemapInfoList.Add(new() { roomSize = tilemapSizes[currentRoom.copyCoordinates.IndexOf(coordinates)], copyCoordinates = coordinates });
        //}
        currentRoom.copyCoordinates.ForEach(x => { tilemapInfoList.Add(new() { roomSize = tilemapSizes[currentRoom.copyCoordinates.IndexOf(x)], copyCoordinates = x }); });
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
                    Vector3Int newTileLocation = new(startPos.x + x + tilemapInfo.copyCoordinates.offsetFromOrigin_x, startPos.y + y + tilemapInfo.copyCoordinates.offsetFromOrigin_y, 0);
                    currentTilemapRef.SetTile(newTileLocation, tileInfo.tileBase);
                }
            }
        }
    }

    public void OnEnterRoom()
    {
        playerController.transform.position = entranceObjInstance.transform.position;
        if(currentRoom is MachineRoomTemplate)
        {
            
            currentRoom_Data = new MachineRoomData();
            MachineRoomTemplate currentMachineRoom = currentRoom as MachineRoomTemplate;
            MachineRoomData machineRoomData = currentRoom_Data as MachineRoomData;
            machineRoomData.startingLifeEnergy = Random.Range(currentMachineRoom.minStartingLifeEnergy, currentMachineRoom.maxStartingLifeEnergy + 1);
            machineRoomData.machinesInRoom = SetMachinesInRoom();
            //Debug.Log(machineRoomData.startingLifeEnergy);
        }
        else if(currentRoom is EnemyRoomTemplate)
        {
            currentRoom_Data = new EnemyRoomData();
            EnemyRoomData enemyRoomData = currentRoom_Data as EnemyRoomData;
            EnemyRoomTemplate currentEnemyRoom = currentRoom as EnemyRoomTemplate;
            enemyRoomData.startingMana = currentEnemyRoom.startingMana;
            enemyRoomData.basicEnemiesCount = Random.Range(currentEnemyRoom.basicEnemiesCount_min, currentEnemyRoom.basicEnemiesCount_max + 1);
            enemyRoomData.intermediateEnemiesCount = Random.Range(currentEnemyRoom.intermediateEnemiesCount_min, currentEnemyRoom.intermediateEnemiesCount_max + 1);
            enemyRoomData.advancedEnemiesCount = Random.Range(currentEnemyRoom.advancedEnemiesCount_min, currentEnemyRoom.advancedEnemiesCount_max + 1);
            enemySpawnHandler.SpawnEnemies();
            
        }
    }
    public void OnExitRoom()
    {
        clearedRoomsCount++;
        if(currentRoom is EnemyRoomTemplate) {
            roomPickerRef.IncreaseRoomsClearedCount(currentRoom as EnemyRoomTemplate);
            enemySpawnHandler.ClearEnemies();
        }
        else
        {
            DestroyMachines();
        }
        roomPickerRef.currentRoomType = (roomPickerRef.currentRoomType == RoomPicker.RoomType.Enemy) ? RoomPicker.RoomType.Machine : RoomPicker.RoomType.Enemy;
        Destroy(exitObjInstance);
        Destroy(entranceObjInstance);
        originalTilemaps.ForEach((x) => x.ClearAllTiles());
        tilemapInfoList.Clear();
        roomPickerRef.PickCurrentRoom();
    }
    
    public void DestroyMachines()
    {
        (currentRoom_Data as MachineRoomData).machinesInRoom.ToList().ForEach((x) => Destroy(x));
        (currentRoom_Data as MachineRoomData).machinesInRoom.Clear();
    }

    public HashSet<GameObject> SetMachinesInRoom()
    {

        HashSet<GameObject> machines = new();
        for(int i = 0; i < (currentRoom as MachineRoomTemplate).machineCoordinates.Count; i++)
        {
            GameObject machinePrefab = (currentRoom as MachineRoomTemplate).machineCoordinates.Select((x) => x.machinePrefab).ToList()[i];
            Vector3 machineLocation = (currentRoom as MachineRoomTemplate).machineCoordinates[i].location - currentRoom.roomOrigin;
            GameObject machineInstance = Instantiate(machinePrefab,machineLocation, Quaternion.identity);
            machineInstance.GetComponent<MachineScript>().roomGenerator = this;
            machineInstance.GetComponent<MachineScript>().playerRef = playerController;
            machines.Add(machineInstance);
        }
        return machines;
    }
    public void CreateEntranceAndExit(Vector3Int startPos)
    {
        currentRoom.SetEntranceLocationOffset(out var entranceOffset);
        currentRoom.SetExitLocationOffset(out var exitOffset);

        exitObjInstance = Instantiate(currentRoom.exitObj, startPos + exitOffset,Quaternion.identity);
        entranceObjInstance = Instantiate(currentRoom.entranceObj, startPos + entranceOffset, Quaternion.identity);
    }

    public void GenerateRoom()
    {
        CalculateRoomSizes();
        GetTileMapTemplateInfo();
        SetTilemapInfo(new());
        CreateEntranceAndExit(new());
        OnEnterRoom();
        StartCoroutine(LoadPathfinder());
    }

    IEnumerator LoadPathfinder()
    {
        yield return new WaitForEndOfFrame();
        pathfinder.data.gridGraph.center = GetMiddleOfFloor();
        float nodeSize = pathfinder.data.gridGraph.nodeSize;
        float multiplier = 2;
        pathfinder.data.gridGraph.SetDimensions(Mathf.FloorToInt((GetSizeOfFloor().x * multiplier) / nodeSize), Mathf.FloorToInt((GetSizeOfFloor().y * multiplier) / nodeSize), nodeSize);
        pathfinder.Scan(pathfinder.data.gridGraph);
    }
    Vector3 GetMiddleOfFloor()
    {
        RoomDataTemplate.TilemapCopyCoordinates floorCoordinates =  currentRoom.copyCoordinates.FirstOrDefault((x) => x.targetTilemap == RoomDataTemplate.TilemapTypes.Floor);
        Vector3 floorStartPos = new(floorCoordinates.offsetFromOrigin_x, floorCoordinates.offsetFromOrigin_y);
        Vector3 floorMiddlePos = floorStartPos + new Vector3((floorCoordinates.maxX - floorCoordinates.minX) * 0.5f, (floorCoordinates.maxY - floorCoordinates.minY) * 0.5f);
        return floorMiddlePos;

    }

    Vector3Int GetSizeOfFloor()
    {
        RoomDataTemplate.TilemapCopyCoordinates floorCoordinates = currentRoom.copyCoordinates.FirstOrDefault((x) => x.targetTilemap == RoomDataTemplate.TilemapTypes.Floor);
        Vector3Int floorSize = new(floorCoordinates.maxX - floorCoordinates.minX, floorCoordinates.maxY - floorCoordinates.minY);
        return floorSize;
    }
    // Start is called before the first frame update
    void Start()
    {
        roomPickerRef = GetComponent<RoomPicker>();
      enemySpawnHandler =  GetComponent<EnemySpawnHandler>();
        templateTilemaps.First().transform.parent.gameObject.SetActive(false);
    }

    public Vector2Int TilemapPositionSeeker()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        return new((int)mousePos.x, (int)mousePos.y);
    }
}
