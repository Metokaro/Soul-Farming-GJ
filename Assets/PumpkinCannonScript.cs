using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PumpkinCannonScript : BaseWeaponScript
{
    public GameObject projectilePool;
    public Transform projectileOrigin;
    public HashSet<GameObject> projectiles = new();
    bool cooldownOn;
    public override void Attack()
    {
        if (cooldownOn)
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

        currentProjectile.GetComponent<PumpkinProjectileScript>().parentScript = this;
        currentProjectile.GetComponent<PumpkinProjectileScript>().OnProjectileSpawn();
    }
    public void Start()
    {
        foreach (var obj in projectilePool.GetComponentsInChildren<Transform>())
        {
            if (obj.gameObject == projectilePool)
            {
                continue;
            }
            projectiles.Add(obj.gameObject);
            obj.gameObject.SetActive(false);
        }
    }
    public void FixedUpdate()
    {
        WhileHolding();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
    }
}
