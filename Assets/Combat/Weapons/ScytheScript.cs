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

        Debug.Log("gib me your soul" + Time.deltaTime);
        GetComponent<Animator>().SetTrigger("OnAttack");
        StartCoroutine(Cooldown());
        IEnumerator Cooldown()
        {
            attackCooldownActive = true;
            yield return new WaitForSeconds(calculatedWeaponSpeed);
            attackCooldownActive = false;
        }
    }

    public void OnAnimationPlay()
    {
        rotateParent = true;
    }
    public void OnAnimationEnd()
    {
        rotateParent = false;
    }

    void Start()
    {
        OnEquip();
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
}
