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
        if (collisionLayers.Contains(LayerMask.LayerToName(collision.transform.gameObject.layer)))
        {
            ReturnToPool();
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
