using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NailProjectileScript : MonoBehaviour
{
    [HideInInspector]public NailgunScript parentScript;
    Rigidbody2D rb;
    string[] collisionLayers = new string[] { "EnemyHitbox", "WallColliders"};
    public void OnProjectileSpawn()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = parentScript.projectileOrigin.transform.right * 10;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string hitColliderLayer = LayerMask.LayerToName(collision.transform.gameObject.layer);
        if (collisionLayers.Contains(hitColliderLayer))
        {
            ReturnToPool();
            if (hitColliderLayer == "EnemyHitbox")
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
