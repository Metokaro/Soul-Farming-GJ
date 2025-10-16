using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Enemy Data", fileName = "New enemy")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public GameObject enemyObjPrefab;
    public enum EnemyLevels
    {
        Basic,
        Intermediate,
        Advanced
    }
    public EnemyLevels enemyLevel;

}
