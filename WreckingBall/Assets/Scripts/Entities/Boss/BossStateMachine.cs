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
        Debug.Log(nameof(BossStateMachine) + " " + nameof(ChangeState) + " " + (currentState != null ? currentState.animBoolName.ToString() : "null") + " " + (newState != null ? newState.animBoolName.ToString() : "null"));
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
