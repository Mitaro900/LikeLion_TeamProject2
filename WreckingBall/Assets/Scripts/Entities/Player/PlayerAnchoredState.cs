using UnityEngine;

public class PlayerAnchoredState : PlayerState
{
    public PlayerAnchoredState(Player player, StateMachine stateMachine, string animBoolName, Player playerReference) : base(player, stateMachine, animBoolName, playerReference)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(km.GetKey(BindingManager.Action.Jump)))
        {
            player.ReleaseRope();
            stateMachine.ChangeState(player.jumpState);
            return;
        }

        if (!player.IsAnchored)
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
