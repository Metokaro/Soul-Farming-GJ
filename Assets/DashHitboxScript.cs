using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashHitboxScript : MonoBehaviour
{
    public DashScript refScript;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(LayerMask.LayerToName(collision.gameObject.layer) == "EnemyHitbox")
        {
            Debug.Log(collision.transform.parent.gameObject);
            collision.transform.parent.GetComponent<BaseAIBehaviour>().TakeDamage(refScript.damage);
        }
    }
}
