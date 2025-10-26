using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoulCleaveProjectileScript : MonoBehaviour
{
    string[] collisionLayers = new string[] { "EnemyHitbox", "WallColliders" };
    [HideInInspector] public SoulCleaveScript refScript;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(refScript.playerRef.directionOrigin.right * refScript.projectileSpeed, ForceMode2D.Impulse);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string hitColliderLayer = LayerMask.LayerToName(collision.transform.gameObject.layer);
        if (collisionLayers.Contains(hitColliderLayer))
        {
            if (hitColliderLayer == "EnemyHitbox")
            {
                BaseAIBehaviour aiScript = collision.transform.parent.GetComponent<BaseAIBehaviour>();
                aiScript.TakeDamage(refScript.damage);
            }
            Destroy(this.gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
