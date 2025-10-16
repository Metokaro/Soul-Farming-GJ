using System.Collections;
using System.Collections.Generic;
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
    //public 

    public AIStateMachine(BaseAIBehaviour _aiScript) 
    { 
        aiScriptRef = _aiScript;
    
    }
}
