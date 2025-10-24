using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static AbilitiesHandler;

public class PlayerController : MonoBehaviour
{
    public int moveSpeed = 3;
    public Transform directionOrigin;
    public PlayerEquipSystem equipSystem;
    [HideInInspector] public float rotateDiff;
    public List<WeaponDataTemplate> obtainableWeapons;
    [HideInInspector] public bool pauseRotation;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public AbilitiesHandler abilitiesHandler;
    [HideInInspector] public PlayerStats playerStats;
    [HideInInspector] public PlayerLevellingSystem playerLevellingSystem;
    int moveDeltaX;
    int moveDeltaY;

    public float souls;
    public float mana;
    public float maxMana;

    public void Move()
    {
        moveDeltaX = (int)Input.GetAxisRaw("Horizontal");
        moveDeltaY = (int)Input.GetAxisRaw("Vertical");
        Vector2 moveDelta = new(moveDeltaX , moveDeltaY );
        equipSystem.currentWeaponObj.GetComponent<Animator>().SetBool("bobbingEnabled", animator.GetBool("isWalking"));
        rb.velocity = moveDelta.normalized * moveSpeed;
    }

    void SetAnimations(Vector3 mousePos)
    {
        animator.SetBool("isWalking", moveDeltaX != 0 || moveDeltaY != 0);
        float spriteRendererPivotY = spriteRenderer.sprite.pivot.y - Mathf.FloorToInt(spriteRenderer.sprite.pivot.y);
        float xDifference = mousePos.x - transform.position.x;
        float yDifference = mousePos.y - (transform.position.y + transform.localScale.y * (0.5f - spriteRendererPivotY));
        if (pauseRotation == false)
        {
            //Debug.Log(spriteRendererPivotY);
            spriteRenderer.flipX = xDifference < 0;
            animator.SetBool("Down", yDifference < 0 - (transform.localScale.y * 0.5f));
            animator.SetBool("Up", yDifference > 0 + (transform.localScale.y * 0.5f));
            equipSystem.currentWeaponObj.GetComponent<SpriteRenderer>().sortingOrder = animator.GetBool("Up") ? 0 : 1;
        }
    }

    public void MouseFaceDirection(out Vector3 mousePosOutput, out float _angle)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        mousePos.z = 0;
        mousePosOutput = mousePos;
        Vector3 lookDir = (mousePos - directionOrigin.transform.position).normalized;
        float angle = MathF.Atan2(lookDir.y,lookDir.x) * Mathf.Rad2Deg;
        _angle = angle;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        equipSystem = new(this, directionOrigin.Find("WeaponParent"));
        equipSystem.EquipNewWeapon(obtainableWeapons.FirstOrDefault((x) => x.weaponName == "Pumpkin Launcher"));
        abilitiesHandler = GetComponent<AbilitiesHandler>();
        abilitiesHandler.InitializeUnlockedAbilities(equipSystem.currentWeaponObj.GetComponent<BaseWeaponScript>().weaponData.unlockedAbilities);
     
      
    }
    private void Awake()
    {
        playerStats = new();
        playerLevellingSystem = new();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        Move(); Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        SetAnimations(mousePos);
       
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
