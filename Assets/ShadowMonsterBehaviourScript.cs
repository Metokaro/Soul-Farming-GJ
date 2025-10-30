using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMonsterBehaviourScript : BaseAIBehaviour, IMoveAnimation
{
    public float damage;
    public float attackSpeed;
    public float preAttackDelay;
    public float postAttackDelay;
    bool cooldownActive;
    public GameObject dashIndicator;
    public GameObject dashHitbox;
    public Transform directionOrigin;
    public float dashSpeed;
          Rigidbody2D rb;
    public override void Attack()
    {
        if(cooldownActive)
        {
            return;
        }
        StartCoroutine(SetDashDirection());
    }
    IEnumerator TriggerPostAttackDelay()
    {
        Stunned(true);
        yield return new WaitForSeconds(postAttackDelay);
        Stunned(false);
    }

    IEnumerator SetDashDirection()
    {
        cooldownActive = true;
        Vector3 lookDir = (target.position - directionOrigin.transform.position).normalized;
        float angle = MathF.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        directionOrigin.eulerAngles = new(0, 0, angle);
        dashIndicator.transform.rotation = directionOrigin.rotation;
        dashIndicator.SetActive(true);
        yield return new WaitForSeconds(preAttackDelay);
        dashIndicator.SetActive(false);
        StartCoroutine(Dash());
    }
    public override void Start()
    {
        base.Start();
        initiallyFacingLeft = false;
      rb = GetComponent<Rigidbody2D>();
    }
    IEnumerator Dash()
    {
         aiPathfinder.enabled = false;
        rb.AddForce(directionOrigin.right * dashSpeed, ForceMode2D.Impulse);
        dashHitbox.SetActive(true);
        yield return new WaitForSeconds(0.51f);rb.velocity = Vector2.zero;
        dashHitbox.SetActive(false);
        aiPathfinder.enabled = true;
        
        StartCoroutine(TriggerPostAttackDelay());
        StartCoroutine(TriggerCooldown());
    }

    IEnumerator TriggerCooldown()
    {
        
        yield return new WaitForSeconds(attackSpeed);
        cooldownActive = false;
    }
}
