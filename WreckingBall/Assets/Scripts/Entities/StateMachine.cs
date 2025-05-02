using UnityEngine;

public class StateMachine
{
    public State currentState { get; private set; }

    public void Initialize(State initialState)
    {
        currentState = initialState;
        currentState.Enter();
    }
    public void ChangeState(State newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
