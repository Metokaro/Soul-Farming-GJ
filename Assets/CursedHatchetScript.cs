using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedHatchetScript : BaseWeaponScript
{
    Animator animator;
    bool pauseRotate;
    bool attackCooldownOn;
    float yChange;
    public override void Attack()
    {
        if(attackCooldownOn)
        {
            return;
        }
        animator.SetTrigger("OnSwing");
        StartCoroutine(Cooldown());
        IEnumerator Cooldown()
        {
            attackCooldownOn = true;
            yield return new WaitForSeconds(calculatedWeaponSpeed);
            attackCooldownOn = false;
        }
    }

    public void OnSwing()
    {
        pauseRotate = true;
    }

    public void OnSwingEnd()
    {
        pauseRotate = false;
    }

    public override void WhileHolding()
    {
        playerRef.MouseFaceDirection(out var mousePos, out float angle);
        if (pauseRotate == false)
        {
            playerRef.directionOrigin.transform.eulerAngles = new(0, 0, angle);
        }

        float rotateDiff = ((playerRef.spriteRenderer.flipX && playerRef.animator.GetBool("Up") == false && playerRef.animator.GetBool("Down") == false) || (playerRef.animator.GetBool("Up") && playerRef.spriteRenderer.flipX == false) || (playerRef.animator.GetBool("Down")) && playerRef.spriteRenderer.flipX) ? -1 : 1;
        playerRef.equipSystem.weaponParent.transform.localScale = new(1, rotateDiff, 1);
        playerRef.pauseRotation = pauseRotate;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        WhileHolding();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();

        }
    }
}
