using UnityEngine;

public class StateMachine
{
    public State currentState { get; private set; }
    public State previousState { get; private set; }

    public void Initialize(State initialState)
    {
        currentState = initialState;
        previousState = null;
        currentState.Enter();
    }
    public void ChangeState(State newState)
    {
        currentState.Exit();
        previousState = currentState;
        currentState = newState;
        currentState.Enter();
    }
}
