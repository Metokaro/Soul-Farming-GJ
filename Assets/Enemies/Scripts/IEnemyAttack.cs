using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  interface IEnemyAttack
{
    public float damage { get; set; }
    public float attackSpeed { get; set; }
    public bool cooldownActive { get; set; }
    public IEnumerator TriggerCooldown()
    {
        cooldownActive = true;
        yield return new WaitForSeconds(attackSpeed);
        cooldownActive = false;
    }
}
