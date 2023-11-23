using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeState : IState
{
    private Enemy parent;

    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {
        parent.Direction = Vector2.zero;
        parent.Reset();
    }

    public void Update()
    {
        parent.Direction = (parent.MyStartPosition - parent.transform.position).normalized;

       

        float distance = Vector2.Distance(parent.MyStartPosition, parent.transform.position);

        if (distance <= 0.1f)
        {
            parent.ChangeState(new IdleState());
        }
    }
}
