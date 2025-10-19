using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/WeaponData", fileName = "New Weapon")]
public class WeaponDataTemplate : ScriptableObject
{
    public string weaponName;
    public float DefaultDamage;
    public float DefaultWeaponSpeed;
    public float lifeEnergyCost;
    public GameObject weaponPrefab;
    public enum WeaponTypes
    {
        Melee = 1,
        Ranged = 2,
    }
    public WeaponTypes WeaponType;
    public List<AbilityDataTemplate> unlockedAbilities;

    
}
