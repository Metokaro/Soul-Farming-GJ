using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodyChainsawScript : BaseWeaponScript
{
    Animator animator;
    public override void Attack()
    {
       
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
        animator.SetBool("activated", Input.GetKey(KeyCode.Mouse0));
        if (animator.GetBool("activated"))
        {

        }
    }
}
