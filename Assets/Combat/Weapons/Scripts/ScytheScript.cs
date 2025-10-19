using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheScript : BaseWeaponScript
{
   public bool rotateParent;
    bool attackCooldownActive;
    public float yChange;
    public override void Attack()
    {
        if (attackCooldownActive)
        {
            return;

        }

        GetComponent<Animator>().SetTrigger("OnAttack");
        StartCoroutine(Cooldown());
        IEnumerator Cooldown()
        {
            attackCooldownActive = true;
            yield return new WaitForSeconds(calculatedWeaponSpeed);
            attackCooldownActive = false;
        }
    }

    public override void WhileHolding()
    {
        playerRef.MouseFaceDirection(out var mousePos, out float angle);

        if (rotateParent == false)
        {
           playerRef.directionOrigin.transform.eulerAngles = new(0, 0, angle);
        }
       float rotateDiff = ((playerRef.spriteRenderer.flipX && playerRef.animator.GetBool("Up") == false && playerRef.animator.GetBool("Down") == false) || (playerRef.animator.GetBool("Up") && playerRef.spriteRenderer.flipX == false) || (playerRef.animator.GetBool("Down")) && playerRef.spriteRenderer.flipX) ? -1 * yChange : 1 * yChange;
        playerRef.equipSystem.weaponParent.transform.localScale = new(1, rotateDiff, 1);
        playerRef.pauseRotation = rotateParent;
    }
    public void OnAnimationPlay()
    {
        rotateParent = true;
    }
    public void OnAnimationEnd()
    {
        rotateParent = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
        yChange = rotateParent ? -1 : 1;
    }
    private void FixedUpdate()
    {
        WhileHolding();
    }
}
