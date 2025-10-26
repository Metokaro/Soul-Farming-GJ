using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BloodyChainsawScript : BaseWeaponScript
{
    Animator animator;
    public bool bloodshotModeOn;
    bool bloodshotCooldownOn;
    public GameObject bloodShotProjectilePrefab;
    public GameObject bloodShotProjectileOrigin;
    public float projectileSpeed = 6f;
    public override void Attack()
    {
        
        if(bloodshotModeOn)
        {
            BloodshotAttack();
        }
        else
        {
            NormalAttack();
        }
    }
    public void BloodshotAttack()
    {
        if(bloodshotCooldownOn)
        {
            return;
        }
        StartCoroutine(Cooldown());

        GameObject bloodshotProjectile = Instantiate(bloodShotProjectilePrefab, bloodShotProjectileOrigin.transform.position,playerRef.directionOrigin.rotation);
        bloodshotProjectile.SetActive(true);
        bloodshotProjectile.GetComponent<BloodshotProjectileScript>().weaponScript = this;
        IEnumerator Cooldown()
        {
            bloodshotCooldownOn = true;
            yield return new WaitForSeconds(calculatedWeaponSpeed);
            bloodshotCooldownOn = false;
        }
    }
    public void NormalAttack()
    {
        if (hitboxObj == null)
        {
            return;
        }
       HitboxSpawnScript hitboxScript = hitboxObj.GetComponent<HitboxSpawnScript>();
        if (hitboxScript.continousHitboxActive)
        {
            return;
        }
        foreach (var obj in hitboxScript.hitboxesInCollider)
        {
            if (obj != null)
            {
                obj.transform.parent.GetComponent<BaseAIBehaviour>().TakeDamage(calculatedDamage);
            }

        }
        StartCoroutine(Cooldown());

        IEnumerator Cooldown()
        {
            hitboxScript.continousHitboxActive = true;
            yield return new WaitForSeconds(calculatedWeaponSpeed);
            hitboxScript.continousHitboxActive = false;
        }
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
            if (hitboxObj == null)
            {
                SpawnHitbox(new(0.7f, 0.25f), transform.position + playerRef.directionOrigin.right * 0.33f, HitboxSpawnScript.HitboxType.Continous);

            }
            else
            {
                Attack();
            }
        }
        else
        {
            if (hitboxObj != null) {
                Destroy(hitboxObj);
                hitboxObj = null;
            }
        }
    }
}
