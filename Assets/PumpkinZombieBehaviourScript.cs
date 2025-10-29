using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinZombieBehaviourScript : BaseAIBehaviour, IMoveAnimation
{
    public float damage;
    public float attackSpeed;
    public float postAttackDelay;
    bool cooldownActive;
    public Transform projectileOrigin;
    public GameObject rottenPumpkinProjectile;
    public float projectileSpeed;
    public override void Attack()
    {
        if (cooldownActive)
        {
            return;
        }
        if (target == null)
        {
            return;
        }
        StartCoroutine(TriggerPostAttackDelay());
        ShootProjectile();
        GetComponent<Animator>().SetTrigger("Attack");

        StartCoroutine(TriggerCooldown());
    }

    public void ShootProjectile()
    {
        Vector3 lookDir = (target.position - projectileOrigin.transform.position).normalized;
        float angle = MathF.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        projectileOrigin.eulerAngles = new(0, 0, angle);
        GameObject rottenPumpkinInstance = Instantiate(rottenPumpkinProjectile,projectileOrigin.position + projectileOrigin.right * 0.4f, projectileOrigin.rotation);
        rottenPumpkinInstance.SetActive(true);
        rottenPumpkinInstance.GetComponent<RottenPumpkinProjectile>().refScript = this;
        rottenPumpkinInstance.GetComponent<RottenPumpkinProjectile>().OnShoot();
    }
    IEnumerator TriggerPostAttackDelay()
    {
        Stunned(true);
        yield return new WaitForSeconds(postAttackDelay);
        Stunned(false);
    }
    IEnumerator TriggerCooldown()
    {
        cooldownActive = true;
        yield return new WaitForSeconds(attackSpeed);
        cooldownActive = false;
    }
}
