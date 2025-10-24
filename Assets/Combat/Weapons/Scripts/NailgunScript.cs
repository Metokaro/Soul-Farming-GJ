using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class NailgunScript : BaseWeaponScript
{
    public GameObject projectilePool;
    public Transform projectileOrigin;
    public HashSet<GameObject> projectiles = new();
    bool cooldownOn;
    public override void Attack()
    {
        if(cooldownOn)
        {
            return;
        }
        StartCoroutine(Cooldown());
        ShootProjectile();
        IEnumerator Cooldown()
        {
            cooldownOn = true; 
            yield return new WaitForSeconds(calculatedWeaponSpeed);
            cooldownOn = false;
        }
    }

    public void ShootProjectile()
    {
        GameObject currentProjectile = projectiles.FirstOrDefault();
        currentProjectile.SetActive(true);
        projectiles.Remove(currentProjectile);
        currentProjectile.transform.SetParent(null);
        currentProjectile.transform.position = projectileOrigin.position;
        
        currentProjectile.GetComponent<NailProjectileScript>().parentScript = this;
        currentProjectile.GetComponent<NailProjectileScript>().OnProjectileSpawn();
    }

    public void Start()
    {
       foreach(var obj in projectilePool.GetComponentsInChildren<Transform>())
        {
            if(obj.gameObject == projectilePool)
            {
                continue;
            }
            projectiles.Add(obj.gameObject);
            obj.gameObject.SetActive(false);
        }
    }
    public override void WhileHolding()
    {
       base.WhileHolding();

    }
    public void Update()
    {
        
        if(Input.GetKey(KeyCode.Mouse0))
        {
            Attack();
        }
    }
    public void FixedUpdate()
    {
        WhileHolding();
    }
}
