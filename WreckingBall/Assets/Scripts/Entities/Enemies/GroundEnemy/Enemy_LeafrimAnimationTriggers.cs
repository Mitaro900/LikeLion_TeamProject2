using UnityEngine;

public class Enemy_LeafrimAnimationTriggers : MonoBehaviour
{
    private Enemy_Leafrim enemy => GetComponentInParent<Enemy_Leafrim>();

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.AttackCheck.position, enemy.AttackCheckRadius);

        foreach (var hit in colliders)
        {
            //if (hit.GetComponent<Player>() != null) ;
            //hit.GetComponent<Player>().Damage();
        }
    }


    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCouterWindow() => enemy.CloseCounterAttackWindow();


}
