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
            CreateAOE();
            ReturnToPool();
        }
    }

    IEnumerator CreateEffect()
    {
        GameObject effectInstance = Instantiate(parentScript.explosionEffect, transform.position, Quaternion.identity);
        effectInstance.SetActive(true);
        yield return new WaitForSeconds(effectInstance.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
        Destroy(effectInstance);
    }

    public void CreateAOE()
    {
        List<RaycastHit2D> hits = Physics2D.CircleCastAll(transform.position, parentScript.aoeSize, Vector2.zero, 0, parentScript.explosionLayerMask).ToList();
        foreach(var hit in hits)
        {
            BaseAIBehaviour aiScript = hit.collider.transform.parent.GetComponent<BaseAIBehaviour>();
            aiScript.TakeDamage(parentScript.calculatedDamage);
        }
       parentScript.StartCoroutine(CreateEffect());
    }

    void ReturnToPool()
    {
        parentScript.projectiles.Add(this.gameObject);
        transform.position = parentScript.projectilePool.transform.position;
        transform.SetParent(parentScript.projectilePool.transform);
        this.gameObject.SetActive(false);
    }

}
