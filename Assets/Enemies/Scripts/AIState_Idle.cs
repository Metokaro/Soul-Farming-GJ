using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState_Idle : BaseAIState
{
    public AIState_Idle(AIStateMachine.AIStates _state) : base(_state)
    {
    }

    public override void EnterState(BaseAIBehaviour aiScript, AIStateMachine.AIStates previousState)
    {
      
    }
}
