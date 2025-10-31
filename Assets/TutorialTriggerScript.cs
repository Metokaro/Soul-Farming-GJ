using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTriggerScript : MonoBehaviour
{
    public List<GameObject> activatedHints;
    public List<GameObject> unactivatedHints;
    public bool damagePlayerOnEnter;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            activatedHints.ForEach((x) => x.SetActive(true));
            unactivatedHints.ForEach((x) => x.SetActive(false));
            if(damagePlayerOnEnter)
            {
                collision.transform.parent.GetComponent<PlayerController>().TakeDamage(15);
            }
            Destroy(gameObject);
        }
    }
}
