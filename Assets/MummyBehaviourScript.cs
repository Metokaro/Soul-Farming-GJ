using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyBehaviourScript : BaseAIBehaviour, IMoveAnimation
{
    public float damage;
    public float attackSpeed;
    public float postAttackDelay;
    bool cooldownActive;

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

        GetComponent<Animator>().SetTrigger("Attack");
        target.GetComponent<PlayerController>().TakeDamage(damage);
        StartCoroutine(TriggerCooldown());
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
