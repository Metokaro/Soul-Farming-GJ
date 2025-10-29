using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RottenPumpkinProjectile : MonoBehaviour
{
    public PumpkinZombieBehaviourScript refScript;
    public void OnShoot()
    {
        GetComponent<Rigidbody2D>().AddForce(refScript.projectileOrigin.right * refScript.projectileSpeed, ForceMode2D.Impulse);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.transform.parent.gameObject.GetComponent<PlayerController>().TakeDamage(refScript.damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("WallColliders"))
        {
            Destroy(gameObject);
        }
    }
}
