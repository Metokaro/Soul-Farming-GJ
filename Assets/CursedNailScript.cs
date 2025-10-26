using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CursedNailScript : MonoBehaviour
{
    [HideInInspector] public NailgunScript parentScript;
    Rigidbody2D rb;
    public void OnProjectileSpawn()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.rotation = parentScript.playerRef.directionOrigin.rotation;
        rb.velocity = parentScript.projectileOrigin.transform.right * parentScript.cursedNailProjectileSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string hitColliderLayer = LayerMask.LayerToName(collision.transform.gameObject.layer);

        if (hitColliderLayer == "EnemyHitbox")
        {
            BaseAIBehaviour aiScript = collision.transform.parent.GetComponent<BaseAIBehaviour>();
            aiScript.TakeDamage(parentScript.cursedNailDamage);
        }
        else if (hitColliderLayer == "WallColliders")
        {
            Destroy(gameObject);
        }
    }
}
