using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState_Retreating : BaseAIState, IUpdate
{
    public AIState_Retreating(AIStateMachine.AIStates _state) : base(_state) {
        UpdatePerFrame = true;
        IsFixedUpdate = true;
    }

    public bool UpdatePerFrame { get; set; }
    public bool IsFixedUpdate { get ; set; }

    public override void EnterState(BaseAIBehaviour aiScript, AIStateMachine.AIStates previousState)
    {
        aiScript.SetEndPoint(aiScript.spawnPosition, AIStateMachine.AIStates.Idle);

    }

    public void OnUpdate(BaseAIBehaviour aiScript)
    {
        aiScript.DetectTargetsInRange();
    }
}
