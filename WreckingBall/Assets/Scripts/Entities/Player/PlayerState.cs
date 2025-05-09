using UnityEngine;

public class PlayerState : State
{
    protected Player player;
    protected BindingManager km;

    protected float xInput;
    protected float yInput;

    public PlayerState(Entity entity, StateMachine stateMachine, string animBoolName, Player player) : base(entity, stateMachine, animBoolName)
    {
        this.player = player;
    }

    public override void Enter()
    {
        base.Enter();

        km = BindingManager.Instance;
    }

    public override void Update()
    {
        base.Update();

        if(Input.GetKey(km.GetKey(BindingManager.Action.Right)) && !Input.GetKey(km.GetKey(BindingManager.Action.Left)))
        {
            xInput = 1f;
        }
        else if (Input.GetKey(km.GetKey(BindingManager.Action.Left)) && !Input.GetKey(km.GetKey(BindingManager.Action.Right)))
        {
            xInput = -1f;
        }
        else
        {
            xInput = 0f;
        }

        if (Input.GetKey(km.GetKey(BindingManager.Action.Up)) && !Input.GetKey(km.GetKey(BindingManager.Action.Down)))
        {
            yInput = 1f;
        }
        else if (Input.GetKey(km.GetKey(BindingManager.Action.Down)) && !Input.GetKey(km.GetKey(BindingManager.Action.Up)))
        {
            yInput = -1f;
        }
        else
        {
            yInput = 0f;
        }

        if (Input.GetKeyDown(km.GetKey(BindingManager.Action.Hook)))
        {
            if (!player.IsRopeActive && !player.IsBusy)
            {
                player.LaunchRope(xInput, yInput);
            }
            else if (player.IsAnchored)
            {
                if (player.isPull)
                {
                    player.ReleaseRope();
                }
                else
                {
                    player.PullRope();
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}
