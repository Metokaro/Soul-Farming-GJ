using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState_Stunned : BaseAIState
{
    public AIState_Stunned(AIStateMachine.AIStates _state) : base(_state)
    {
    }

    public override void EnterState(BaseAIBehaviour aiScript, AIStateMachine.AIStates previousState)
    {
        aiScript.destinationSetter.target = aiScript.transform;
        if (aiScript is IMoveAnimation)
        {
            (aiScript as IMoveAnimation).Move(false, aiScript);
        }
    }
    
}
