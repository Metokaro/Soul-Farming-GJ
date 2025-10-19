using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPointScript : MonoBehaviour
{
    public GameObject movingAgent;
    public AIStateMachine.AIStates stateAfterReachingPoint;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == movingAgent && collision.gameObject.CompareTag("Enemy"))
        {
            movingAgent.GetComponent<BaseAIBehaviour>().aiStateMachine.SetState(stateAfterReachingPoint);
            Destroy(this.gameObject);
        }
        
    }
}
