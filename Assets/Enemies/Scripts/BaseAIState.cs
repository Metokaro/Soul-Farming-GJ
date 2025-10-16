using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAIState
{
    public AIStateMachine.AIStates state;
    public abstract void EnterState(BaseAIBehaviour aiScript,AIStateMachine.AIStates previousState);
    public virtual void ExitState(BaseAIBehaviour aiScript) { }

    public BaseAIState(AIStateMachine.AIStates _state)
    {
        state = _state;
    }
}

public interface IUpdate
{
    public bool UpdatePerFrame {  get; set; }
    public bool IsFixedUpdate {get; set; }
    public abstract void OnUpdate();
}