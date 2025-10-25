using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PumpkinProjectileScript : MonoBehaviour
{
    [HideInInspector] public PumpkinCannonScript parentScript;
    Rigidbody2D rb;
    string[] collisionLayers = new string[] { "EnemyHitbox", "WallColliders" };
    public void OnProjectileSpawn()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = parentScript.projectileOrigin.transform.right * 7;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string hitColliderLayer = LayerMask.LayerToName(collision.transform.gameObject.layer);
        if (collisionLayers.Contains(hitColliderLayer))
        {
            ReturnToPool();
            if(hitColliderLayer == "EnemyHitbox")
            {
                BaseAIBehaviour aiScript = collision.transform.parent.GetComponent<BaseAIBehaviour>();
                aiScript.TakeDamage(parentScript.calculatedDamage);
            }
        }
    }
    void ReturnToPool()
    {
        parentScript.projectiles.Add(this.gameObject);
        transform.position = parentScript.projectilePool.transform.position;
        transform.SetParent(parentScript.projectilePool.transform);
        this.gameObject.SetActive(false);
    }

}
