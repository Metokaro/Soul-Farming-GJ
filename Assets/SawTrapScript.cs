using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawTrapScript : MonoBehaviour
{
    public SawTrapAbilityScript refScript;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(LayerMask.LayerToName(collision.gameObject.layer) == "Enemy")
        {
           refScript.TriggerTrap(collision.gameObject);
            Destroy(gameObject);
        }
    }
  
}
