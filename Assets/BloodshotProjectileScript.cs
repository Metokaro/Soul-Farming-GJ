using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BloodshotProjectileScript : MonoBehaviour
{
    string[] collisionLayers = new string[] { "EnemyHitbox", "WallColliders" };
    [HideInInspector] public BloodshotModeScript abilityRefScript;
    [HideInInspector] public BloodyChainsawScript weaponScript;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(weaponScript.playerRef.directionOrigin.right * weaponScript.projectileSpeed, ForceMode2D.Impulse);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string hitColliderLayer = LayerMask.LayerToName(collision.transform.gameObject.layer);
        if (collisionLayers.Contains(hitColliderLayer))
        {
            if (hitColliderLayer == "EnemyHitbox")
            {
                BaseAIBehaviour aiScript = collision.transform.parent.GetComponent<BaseAIBehaviour>();
                aiScript.TakeDamage(weaponScript.calculatedDamage);
            }
            Destroy(this.gameObject);
        }
    }
}
