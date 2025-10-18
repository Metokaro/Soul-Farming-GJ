using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState_Pursuing : BaseAIState, IUpdate
{
    public AIState_Pursuing(AIStateMachine.AIStates _state) : base(_state)
    {
        UpdatePerFrame = true;
        IsFixedUpdate = false;
    }

  public  bool UpdatePerFrame { get; set; }
  public  bool IsFixedUpdate { get; set; }

    public override void EnterState(BaseAIBehaviour aiScript, AIStateMachine.AIStates previousState)
    {
        
    }

    public void OnUpdate(BaseAIBehaviour aiScript)
    {
        float distanceFromTarget = Vector2.Distance(aiScript.transform.position, aiScript.destinationSetter.target.position);
        if(aiScript.pursueDistance < distanceFromTarget)
        {
            aiScript.destinationSetter.target = null;
            aiScript.aiStateMachine.SetState(AIStateMachine.AIStates.Retreating);
        }
    }
}
