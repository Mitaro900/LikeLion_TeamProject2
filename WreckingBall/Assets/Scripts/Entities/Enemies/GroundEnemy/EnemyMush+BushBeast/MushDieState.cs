using UnityEngine;

public class MushDieState : State
{
    private Enemy_Mush enemy;

    public MushDieState(Entity entity, StateMachine stateMachine, string animBoolName, Enemy_Mush enemy) : base(entity, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.enabled = false;

        foreach (var col in enemy.GetComponentsInChildren<Collider2D>())
            col.enabled = false;

        enemy.transform.SetParent(null);

        enemy.rb.bodyType = RigidbodyType2D.Dynamic;
        enemy.SetZeroVelocity();

        float knockbackHorizontal = 20f;
        float knockbackVertical = 20f;
        enemy.rb.linearVelocity = new Vector2(-enemy.facingDir * knockbackHorizontal, knockbackVertical);

        enemy.anim.SetTrigger("Die");

        Object.Destroy(enemy.gameObject, 3f);

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
    
}
