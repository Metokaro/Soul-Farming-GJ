using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawnHandler : MonoBehaviour
{
    RoomGenerator roomGeneratorRef;
    public HashSet<EnemyData> basicEnemiesData = new();
    public HashSet<EnemyData> intermediateEnemiesData = new();
    public HashSet<EnemyData> advancedEnemiesData = new();
    int minSpawnRange_x, minSpawnRange_y, maxSpawnRange_x, maxSpawnRange_y;
    public LayerMask spawnableAreaLayerMask;
    public LayerMask unspawnableAreaLayerMask;
    public void SortEnemyData()
    {
        EnemyRoomTemplate enemyRoom = (roomGeneratorRef.currentRoom as EnemyRoomTemplate);
        basicEnemiesData.AddRange(enemyRoom.enemies.Where((x) => x.enemyLevel == EnemyData.EnemyLevels.Basic));
        intermediateEnemiesData.AddRange(enemyRoom.enemies.Where((x) => x.enemyLevel == EnemyData.EnemyLevels.Intermediate));
        advancedEnemiesData.AddRange(enemyRoom.enemies.Where((x) => x.enemyLevel == EnemyData.EnemyLevels.Advanced));
    }

    public void SetSpawnArea()
    {
       RoomDataTemplate.TilemapCopyCoordinates floorTilemapCoordinates = roomGeneratorRef.currentRoom.copyCoordinates.First((x) => x.targetTilemap == RoomDataTemplate.TilemapTypes.Floor);
        minSpawnRange_x = (floorTilemapCoordinates.minX + floorTilemapCoordinates.offsetFromOrigin_x) - (int)roomGeneratorRef.currentRoom.roomOrigin.x + 1;
        minSpawnRange_y = (floorTilemapCoordinates.minY + floorTilemapCoordinates.offsetFromOrigin_y) - (int)roomGeneratorRef.currentRoom.roomOrigin.y + 1;
        maxSpawnRange_x = (floorTilemapCoordinates.maxX + floorTilemapCoordinates.offsetFromOrigin_x) - (int)roomGeneratorRef.currentRoom.roomOrigin.x - 2;
        maxSpawnRange_y = (floorTilemapCoordinates.maxY + floorTilemapCoordinates.offsetFromOrigin_y) - (int)roomGeneratorRef.currentRoom.roomOrigin.y;
        //Debug.Log(" minX: " + minSpawnRange_x + " maxX: " + maxSpawnRange_x +  "minY:" + minSpawnRange_y + " maxY" + maxSpawnRange_y);
    }

    public void TriggerGroupSpawn()
    {
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForEndOfFrame();
            if (roomGeneratorRef.currentRoom_Data is RoomGenerator.EnemyRoomData)
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
    }


    public void SpawnRandomEnemy(List<EnemyData> enemyOptions)
    {
        if(enemyOptions.Count < 1)
        {
            return;
        }
        float randomX = Random.Range(minSpawnRange_x, maxSpawnRange_x) +0.5f;
        float randomY = Random.Range(minSpawnRange_y, maxSpawnRange_y)+0.5f;
        EnemyData randomEnemy = enemyOptions[Random.Range(0, enemyOptions.Count)];
        Vector3 spawnLocation = new(randomX, randomY, 0);
        if (CheckIfInSpawnableArea(spawnLocation) == false || CheckIfCollider(spawnLocation))
        {

            SpawnRandomEnemy(enemyOptions);
            return;
        }
        GameObject enemyObjInstance = Instantiate(randomEnemy.enemyObjPrefab, spawnLocation, Quaternion.identity);
        enemyObjInstance.GetComponent<BaseAIBehaviour>().enemyData = randomEnemy;
        (roomGeneratorRef.currentRoom_Data as RoomGenerator.EnemyRoomData).enemiesInRoom.Add(enemyObjInstance);

    }
    bool CheckIfInSpawnableArea(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position,Vector3.zero, 1, spawnableAreaLayerMask);
        //Debug.Log(hit.collider);
        return hit;
    }
    bool CheckIfCollider(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector3.zero, 1, unspawnableAreaLayerMask);
        return hit;
    }
    public void ClearEnemies()
    {
        foreach (GameObject enemy in (roomGeneratorRef.currentRoom_Data as RoomGenerator.EnemyRoomData).enemiesInRoom)
        {
            Destroy(enemy);
        }
        (roomGeneratorRef.currentRoom_Data as RoomGenerator.EnemyRoomData).enemiesInRoom.Clear();
    }
    //public void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.Mouse0))
    //    {
    //        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector3.zero, 1, spawnableAreaLayerMask);
    //        Debug.Log(hit.collider);
    //    }
    //}
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            ClearEnemies();
        }
    }
    public void SpawnEnemies()
    {
        roomGeneratorRef = GetComponent<RoomGenerator>();
        SortEnemyData();
        SetSpawnArea();
        TriggerGroupSpawn();
    }
}
