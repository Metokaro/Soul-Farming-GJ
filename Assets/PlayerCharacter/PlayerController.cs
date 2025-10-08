using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int moveSpeed = 3;
    public Transform directionOrigin;
    public Transform weaponParent;
    public GameObject weaponObj;
    Animator animator;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

   [HideInInspector] public PlayerStats playerStats = new();
    int moveDeltaX;
    int moveDeltaY;

    public void Move()
    {
        moveDeltaX = (int)Input.GetAxisRaw("Horizontal");
        moveDeltaY = (int)Input.GetAxisRaw("Vertical");
        Vector2 moveDelta = new(moveDeltaX , moveDeltaY );
        animator.SetBool("isWalking", moveDeltaX != 0 || moveDeltaY != 0);
        weaponObj.GetComponent<Animator>().SetBool("bobbingEnabled", animator.GetBool("isWalking"));
        rb.velocity = moveDelta.normalized * moveSpeed;
    }

    public void MouseFaceDirection()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        mousePos.z = 0;
        Vector3 lookDir = (mousePos - directionOrigin.transform.position).normalized;
        float angle = MathF.Atan2(lookDir.y,lookDir.x) * Mathf.Rad2Deg;
      
        directionOrigin.transform.eulerAngles = new(0, 0, angle);
        float xDifference =  mousePos.x - transform.position.x;
        spriteRenderer.flipX = xDifference < 0;
        float rotateDiff = spriteRenderer.flipX ? -1 : 1;
        weaponParent.transform.localScale = new(1, rotateDiff,1);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
      
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        MouseFaceDirection();
    }
}
