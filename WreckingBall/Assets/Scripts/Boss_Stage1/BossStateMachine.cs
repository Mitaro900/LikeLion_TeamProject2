using UnityEngine;

public class BossStateMachine
{
    public BossState currentState { get; private set; }

    public void Initialize(BossState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(BossState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
