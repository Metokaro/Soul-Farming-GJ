using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIStateMachine
{
    public BaseAIBehaviour aiScriptRef;
    public enum AIStates
    {
        Idle,
        Pursuing,
        Retreating
    }
    public AIStates currentAIState;
    public AIStates prevAIState;
    public List<BaseAIState> aiStateObjList;
    public BaseAIState currentAIStateObj;

    public void SetState(AIStates state)
    {
        if(currentAIStateObj != null)
        {
            prevAIState = currentAIStateObj.state;
            currentAIStateObj.ExitState(aiScriptRef);
        }
        currentAIStateObj = aiStateObjList.FirstOrDefault((x) => x.state == state);
        currentAIState = state;
        currentAIStateObj.EnterState(aiScriptRef,prevAIState);
        Debug.Log(currentAIState + ":" + currentAIStateObj);
    }

    public void UpdateState(bool perFrame, bool isFixedUpdate)
    {
        if(currentAIStateObj is IUpdate)
        {
            if (perFrame == false)
            {
                (currentAIStateObj as IUpdate).OnUpdate(aiScriptRef);
            }
            else if ((currentAIStateObj as IUpdate).UpdatePerFrame)
            {
                Debug.Log("Perframe");
                if (isFixedUpdate == false)
                {
                    (currentAIStateObj as IUpdate).OnUpdate(aiScriptRef);
                }
                else if ((currentAIStateObj as IUpdate).IsFixedUpdate)
                {
                    Debug.Log("PerframeFU");
                    (currentAIStateObj as IUpdate).OnUpdate(aiScriptRef);
                }
            }
        }
    }

    public AIStateMachine(BaseAIBehaviour _aiScript) 
    { 
        aiStateObjList = new() { new AIState_Idle(AIStates.Idle),new AIState_Pursuing(AIStates.Pursuing), new AIState_Retreating(AIStates.Retreating) };
        aiScriptRef = _aiScript;
    
    }
}
