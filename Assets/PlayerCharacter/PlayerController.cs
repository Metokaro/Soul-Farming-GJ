using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AbilitiesHandler;

public class PlayerController : MonoBehaviour
{
    public int moveSpeed = 3;
    public Transform directionOrigin;
    public Transform weaponParent;
    public GameObject weaponObj;

    Animator animator;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    public AbilitiesHandler abilitiesHandler;
   [HideInInspector] public PlayerStats playerStats;
    [HideInInspector] public PlayerLevellingSystem playerLevellingSystem;
    int moveDeltaX;
    int moveDeltaY;

    public void Move()
    {
        moveDeltaX = (int)Input.GetAxisRaw("Horizontal");
        moveDeltaY = (int)Input.GetAxisRaw("Vertical");
        Vector2 moveDelta = new(moveDeltaX , moveDeltaY );
        weaponObj.GetComponent<Animator>().SetBool("bobbingEnabled", animator.GetBool("isWalking"));
        rb.velocity = moveDelta.normalized * moveSpeed;
    }

    void SetAnimations(Vector3 mousePos)
    {
        animator.SetBool("isWalking", moveDeltaX != 0 || moveDeltaY != 0);
        float xDifference = mousePos.x - transform.position.x;
        float yDifference = mousePos.y - transform.position.y;
        spriteRenderer.flipX = xDifference < 0;
        animator.SetBool("Down", yDifference + (transform.localScale.y * 0.5f) < 0);
    }

    public void MouseFaceDirection(out Vector3 mousePosout)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        mousePos.z = 0;
        mousePosout = mousePos;
        Vector3 lookDir = (mousePos - directionOrigin.transform.position).normalized;
        float angle = MathF.Atan2(lookDir.y,lookDir.x) * Mathf.Rad2Deg;
      
        directionOrigin.transform.eulerAngles = new(0, 0, angle);
      
        float rotateDiff = spriteRenderer.flipX ? -1 : 1;
        weaponParent.transform.localScale = new(1, rotateDiff,1);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        abilitiesHandler.InitializeUnlockedAbilities(weaponObj.GetComponent<BaseWeaponScript>().weaponData.unlockedAbilities);
     
      
    }
    private void Awake()
    {
        playerStats = new();
        playerLevellingSystem = new();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        Move();
        MouseFaceDirection(out var MousePos) ;
        SetAnimations(MousePos);
       
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            playerLevellingSystem.IncreaseExp(3);
        }
        if (abilitiesHandler.abilities.Count > 0)
        {
            foreach (Ability ability in abilitiesHandler.abilities)
            {
                if (Input.GetKeyDown(ability.keybind))
                {
                    ability.abilityScript.OnAbilityCast();
                }
            }
        }
       
    }
}
