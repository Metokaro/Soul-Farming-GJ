using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static AbilitiesHandler;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3;
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
    public LayerMask targetableLayerMask;
    public float souls;
    [HideInInspector] public float mana;
    public float maxMana;
    [HideInInspector]public float health;
    public float maxHealth;
    [HideInInspector] public PlayerScreenScript playerScreenRef;
    [HideInInspector] public bool canMove;
    [HideInInspector] public bool canTakeDamage;
    [HideInInspector] public delegate void OnTakeDamageFunction();
    [HideInInspector] public OnTakeDamageFunction onTakeDamageFunction;
    Color defaultColor;
    public void Move()
    {
        if(canMove == false)
        {
            return;
        }
        moveDeltaX = (int)Input.GetAxisRaw("Horizontal");
        moveDeltaY = (int)Input.GetAxisRaw("Vertical");
        Vector2 moveDelta = new(moveDeltaX , moveDeltaY );
        
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

    public void DepleteMana(float amount)
    {
        mana -= amount;
        playerScreenRef.UpdateManaBar();
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
        canTakeDamage = true;
        health = maxHealth;
        
        mana = maxMana * 0.25f;
        playerScreenRef.UpdateCurrencies(souls, 0, false);
        playerScreenRef.UpdateManaBar();
        defaultColor =  spriteRenderer.color;

    }
    private void Awake()
    {
        canMove = true;
        playerStats = new();
        playerLevellingSystem = new();
    }

    public void TakeDamage(float damageTaken)
    {
        onTakeDamageFunction?.Invoke();
        StartCoroutine(DamageIndicator());
        if (canTakeDamage == false)
        { return; }
        damageTaken -= Mathf.RoundToInt(damageTaken * (playerStats.defenseMultiplier * 0.15f));
        health -= damageTaken;
        if(health <= 0 ) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        playerScreenRef.UpdateHealthBar();
    }

    IEnumerator DamageIndicator()
    {
        spriteRenderer.color = Color.white;
        spriteRenderer.color = canTakeDamage ? Color.red : new(1,1,1,0.4f);
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = defaultColor;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        Move(); 
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        SetAnimations(mousePos);
       
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            TakeDamage(20);
        }
        CheckForAbilities();
       
    }

    void CheckForAbilities()
    {
        if (abilitiesHandler.abilities.Count < 1)
        {
            return;
        }
        foreach (Ability ability in abilitiesHandler.abilities)
        {
            if (Input.GetKeyDown(ability.keybind))
            {
                if (ability.abilityData.manaCost > mana)
                {
                    return;
                }
                ability.abilityScript.OnAbilityCast();
                DepleteMana(ability.abilityData.manaCost);
            }
        }
    }
}
