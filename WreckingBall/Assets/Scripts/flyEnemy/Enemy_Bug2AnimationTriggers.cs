using UnityEngine;

public class Enemy_Bug2AnimationTriggers : MonoBehaviour
{
    private Enemy_Bug2 enemy => GetComponentInParent<Enemy_Bug2>();
    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var hit in colliders)
        {
            //if (hit.GetComponent<Player>() != null) ;
            //hit.GetComponent<Player>().Damage();
        }
    }
}
