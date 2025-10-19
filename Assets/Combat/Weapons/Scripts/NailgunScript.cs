using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailgunScript : BaseWeaponScript
{
    public override void Attack()
    {
        Debug.Log("pewpew");
    }
    public override void WhileHolding()
    {
       base.WhileHolding();

    }
    public void Update()
    {
        
        if(Input.GetKey(KeyCode.Mouse0))
        {
            Attack();
        }
    }
    public void FixedUpdate()
    {
        WhileHolding();
    }
}
