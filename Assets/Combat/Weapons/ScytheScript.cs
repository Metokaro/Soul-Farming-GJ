using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheScript : BaseWeaponScript
{
    bool attackCooldownActive;
    public override void Attack()
    {
        if (attackCooldownActive)
        {
            return;

        }

        Debug.Log("gib me your soul" + Time.deltaTime);

        StartCoroutine(Cooldown());
        IEnumerator Cooldown()
        {
            attackCooldownActive = true;
            yield return new WaitForSeconds(calculatedWeaponSpeed);
            attackCooldownActive = false;
        }
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
    }
}
