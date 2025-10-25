using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxSpawnScript : MonoBehaviour
{
    public BaseWeaponScript parentScript;
    public enum HitboxType
    {
        OneHit,
        Continous
    }
    public bool continousHitboxActive;
    public HitboxType hitboxType;
    public HashSet<GameObject> hitboxesInCollider = new();
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) != "EnemyHitbox")
        {
            return;
        }
        if (hitboxType == HitboxType.OneHit)
        {
            BaseAIBehaviour baseAIBehaviour = collision.transform.parent.GetComponent<BaseAIBehaviour>();
            baseAIBehaviour.TakeDamage(parentScript.calculatedDamage);
        }
        else if (hitboxType == HitboxType.Continous)
        {
            hitboxesInCollider.Add(collision.gameObject);
        }

    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) != "EnemyHitbox")
        {
            return;
        }
        if (hitboxType == HitboxType.Continous)
        {
            hitboxesInCollider.RemoveWhere((x) => x == collision.gameObject);
        }
    }
}
