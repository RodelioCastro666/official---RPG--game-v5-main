using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private Enemy parent;

    private float attackCooldown = 3;

    private float extraRange = .5f;

    public void Enter(Enemy parent)
    {
        this.parent = parent;
        parent.MyRigidbody.velocity = Vector2.zero;
        parent.Direction = Vector2.zero;
    }

    

    public void Exit()
    {
       
    }

    public void Update()
    {
        if (parent.MyAttackTime >= attackCooldown &&  !parent.IsAttacking)
        {
            parent.MyAttackTime = 0;

            parent.StartCoroutine(Attack());
        }

        if (parent.MyTarget != null)
        {
            float distance = Vector2.Distance(parent.MyTarget.transform.parent.position, parent.transform.parent.position);

            if (distance >= parent.MyAttackRange + extraRange && !parent.IsAttacking)
            {
                parent.ChangeState(new FollowState());
            }
        }
        else
        {
            parent.ChangeState(new IdleState());
        }
    }

    public IEnumerator Attack()
    {
        parent.IsAttacking = true;

        parent.MyAnimator.SetTrigger("attack");

        yield return new WaitForSeconds(parent.MyAnimator.GetCurrentAnimatorStateInfo(2).length);

        parent.IsAttacking = false;
    }
}
  