using UnityEngine;

public class PlayerState : State
{
    protected Player player;

    protected float xInput;
    protected float yInput;

    public PlayerState(Entity entity, StateMachine stateMachine, string animBoolName, Player player) : base(entity, stateMachine, animBoolName)
    {
        this.player = player;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if(Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            xInput = 1f;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            xInput = -1f;
        }
        else
        {
            xInput = 0f;
        }

        if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
        {
            yInput = 1f;
        }
        else if (Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.UpArrow))
        {
            yInput = -1f;
        }
        else
        {
            yInput = 0f;
        }

        if (Input.GetKeyDown(KeyCode.X))
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
