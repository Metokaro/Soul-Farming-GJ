using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawnHandler : MonoBehaviour
{
    public List<EnemyData> enemyDataList;
    RoomGenerator roomGeneratorRef;
    public HashSet<EnemyData> basicEnemiesData = new();
    public HashSet<EnemyData> intermediateEnemiesData = new();
    public HashSet<EnemyData> advancedEnemiesData = new();
    int minSpawnRange_x, minSpawnRange_y, maxSpawnRange_x, maxSpawnRange_y;
    public LayerMask unspawnableAreaLayerMask;

    public void SortEnemyData()
    {
        basicEnemiesData.AddRange(enemyDataList.Where((x) => x.enemyLevel == EnemyData.EnemyLevels.Basic));
        intermediateEnemiesData.AddRange(enemyDataList.Where((x) => x.enemyLevel == EnemyData.EnemyLevels.Intermediate));
        advancedEnemiesData.AddRange(enemyDataList.Where((x) => x.enemyLevel == EnemyData.EnemyLevels.Advanced));
    }

    public void SetSpawnArea()
    {
       RoomDataTemplate.TilemapCopyCoordinates floorTilemapCoordinates = roomGeneratorRef.currentRoom.copyCoordinates.First((x) => x.targetTilemap == RoomDataTemplate.TilemapTypes.Floor);
        minSpawnRange_x = (floorTilemapCoordinates.minX + floorTilemapCoordinates.offsetFromOrigin_x) - (int)roomGeneratorRef.currentRoom.roomOrigin.x + 1;
        minSpawnRange_y = (floorTilemapCoordinates.minY + floorTilemapCoordinates.offsetFromOrigin_y) - (int)roomGeneratorRef.currentRoom.roomOrigin.y + 2;
        maxSpawnRange_x = (floorTilemapCoordinates.maxX + floorTilemapCoordinates.offsetFromOrigin_x) - (int)roomGeneratorRef.currentRoom.roomOrigin.x - 1;
        maxSpawnRange_y = (floorTilemapCoordinates.maxY + floorTilemapCoordinates.offsetFromOrigin_y) - (int)roomGeneratorRef.currentRoom.roomOrigin.y + 2;
        Debug.Log("minX: " + minSpawnRange_x + "maxX: " + maxSpawnRange_x + "minY:" + minSpawnRange_y + "maxY" + maxSpawnRange_y);
    }

    public void TriggerGroupSpawn()
    {
        if(roomGeneratorRef.currentRoom_Data is RoomGenerator.EnemyRoomData)
        {
            RoomGenerator.EnemyRoomData enemyRoomData = (roomGeneratorRef.currentRoom_Data as RoomGenerator.EnemyRoomData);
            for (int i = 0; i < enemyRoomData.basicEnemiesCount; i++)
            {
                SpawnRandomEnemy(basicEnemiesData.ToList());
            }
            for (int i = 0; i < enemyRoomData.intermediateEnemiesCount; i++)
            {
                SpawnRandomEnemy(intermediateEnemiesData.ToList());
            }
            for (int i = 0; i < enemyRoomData.advancedEnemiesCount; i++)
            {
                SpawnRandomEnemy(advancedEnemiesData.ToList());
            }
        }
       
    }

    public void SpawnRandomEnemy(List<EnemyData> enemyOptions)
    {
        int randomX = Random.Range(minSpawnRange_x, maxSpawnRange_x);
        int randomY = Random.Range(minSpawnRange_y, maxSpawnRange_y);
        EnemyData randomEnemy = enemyOptions[Random.Range(0, enemyOptions.Count)];
        Vector3 spawnLocation = new Vector3(randomX, randomY, 0);
        if(CheckIfCollider(spawnLocation))
        {
            SpawnRandomEnemy(enemyOptions);
            return;
        }
        GameObject enemyObjInstance = Instantiate(randomEnemy.enemyObjPrefab, spawnLocation, Quaternion.identity);
        

    }
    bool CheckIfCollider(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector3.zero, 1, unspawnableAreaLayerMask);
        return hit;
    }

    public void SpawnEnemies()
    {
        SetSpawnArea();
        TriggerGroupSpawn();
    }

    public void Start()
    {
        SortEnemyData();

        roomGeneratorRef = GetComponent<RoomGenerator>();

    }
}
