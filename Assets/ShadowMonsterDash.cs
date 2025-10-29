using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMonsterDash : MonoBehaviour
{
    public ShadowMonsterBehaviourScript refScript;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.transform.parent.GetComponent<PlayerController>().TakeDamage(refScript.damage);
        }
    }
}
