using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KawaiiGhoulBehaviourScript : BaseAIBehaviour, IMoveAnimation
{
    public float damage;
    public float attackSpeed;
    public float preAttackDelay;
    public float postAttackDelay;
    bool cooldownActive;
    Animator animator;
    public Transform directionOrigin;
    GameObject attackhitbox;
    public GameObject recieverHitbox;
    public override void Attack()
    {
        base.Attack();
        if(cooldownActive)
        {
            return;
        }
        Reveal();
        StartCoroutine(TriggerCooldown());
        
        
        
    }

    public void GoInvisible()
    {
        animator.SetTrigger("Invisible");

    }

    public void OnInvisible()
    {
        healthDisplay.gameObject.SetActive(false);
        recieverHitbox.SetActive(false);
    }
    public void OnReveal()
    {
        healthDisplay.gameObject.SetActive(true);
       recieverHitbox.SetActive(true);
    }
    public void Reveal()
    {
        animator.SetTrigger("Reveal");

    }
    public override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
       
    }


    public void Awake()
    {
        onPursueFunctions = GoInvisible;
    }

    void CreateAttackHitbox()
    {
        Vector3 lookDir = (target.position - directionOrigin.transform.position).normalized;
        float angle = MathF.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        directionOrigin.eulerAngles = new(0, 0, angle);
        attackhitbox = new();
        attackhitbox.transform.position = directionOrigin.position + directionOrigin.right * 0.35f;
        attackhitbox.transform.rotation = directionOrigin.rotation;
        attackhitbox.AddComponent<EnemyAttackHitbox>().damage = damage;
        BoxCollider2D collider = attackhitbox.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        collider.size = new(0.5f, 0.5f);

    }
    public void DestroyHitbox()
    {
            Destroy(attackhitbox);
    }
    public IEnumerator TriggerCooldown()
    {
        
        
        cooldownActive = true;
        yield return new WaitForSeconds(attackSpeed);
        cooldownActive = false;
    }
}
