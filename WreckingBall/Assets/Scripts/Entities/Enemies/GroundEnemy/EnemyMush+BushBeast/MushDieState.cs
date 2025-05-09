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
        // 1) 플레이어의 자식에서 해제 (더 이상 끌려가지 않도록)
        enemy.transform.SetParent(null);
        
        // 2) 물리 완전 중지: 속도 0, Kinematic 전환
        enemy.rb.linearVelocity = Vector2.zero;
        enemy.rb.bodyType = RigidbodyType2D.Kinematic;
        
        // 3) 충돌/트리거 비활성화
        enemy.cd.enabled = false;
        
        // 4) Grabbed 상태 해제 (Player 쪽에서 참고할 수 있다면)
        // enemy.IsGrabbed = false;  // Enemy 클래스에 있으면 활성화
        
        // Die 애니메이션 재생
        enemy.anim.SetTrigger("Die");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        Object.Destroy(enemy.gameObject,0.8f);
    }
    
}
