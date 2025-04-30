using UnityEngine;

public class GroundEnemyStateMachine
{
    public GroundEnemyState G_currentState { get; private set; }

    public void Initialize(GroundEnemyState _G_startState)
    {
        G_currentState = _G_startState;
        G_currentState.Enter();
    }

    public void ChangeState(GroundEnemyState _G_newState)
    {
        G_currentState.Exit();
        G_currentState = _G_newState;
        G_currentState.Enter();
    }

}