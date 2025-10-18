using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIState_Idle : BaseAIState, IUpdate
{
    public AIState_Idle(AIStateMachine.AIStates _state) : base(_state)
    {
        UpdatePerFrame = true;
        IsFixedUpdate = true;
    }

    public bool UpdatePerFrame { get; set ; }
    public bool IsFixedUpdate { get; set; }

    public override void EnterState(BaseAIBehaviour aiScript, AIStateMachine.AIStates previousState)
    {
        aiScript.destinationSetter.target = aiScript.transform;
    }

    public void OnUpdate(BaseAIBehaviour aiScript)
    {
        aiScript.DetectTargetsInRange();
    }

    
}
