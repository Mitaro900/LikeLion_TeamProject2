using UnityEngine;

public class EnemyAimationTriggers : MonoBehaviour
{
    private Enemy_Bat enemy => GetComponentInParent<Enemy_Bat>();

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var hit in colliders)
        {
            //if (hit.GetComponent<Player>() != null)
            //    hit.GetComponent<Player>().Damage();
        }
    }
}
