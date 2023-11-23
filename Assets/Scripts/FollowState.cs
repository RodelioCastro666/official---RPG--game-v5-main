using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class FollowState : IState
{
    private Enemy parent;

    private Vector3 offset;

    public void Enter(Enemy parent)
    {
        Player.MyInstance.AddAttackers(parent);
        this.parent = parent;
        parent.MyPath = null; 
    }

    public void Exit()
    {
        parent.Direction = Vector2.zero;
        parent.MyRigidbody.velocity = Vector2.zero;
    }

    public void Update()
    {
       

        if (parent.MyTarget != null)
        {
            parent.Direction = ((parent.MyTarget.transform.position + offset) - parent.transform.position).normalized;
           

            float distance = Vector2.Distance(parent.MyTarget.transform.position + offset, parent.transform.position);

            string animName = parent.MySpriteRenderer.sprite.name;



            if (animName.Contains("right"))
            {
                offset = new Vector3(0.5f, 0.8f);
            }
            else if (animName.Contains("left"))
            {
                offset = new Vector3(-0.5f, 0.8f);
            }
            else if (animName.Contains("up"))
            {
                offset = new Vector3(0f, 1.2f);
            }
            else if (animName.Contains("down"))
            {
                offset = new Vector3(0f, 0);
            }

            if (distance <= parent.MyAttackRange)
            {
                parent.ChangeState(new AttackState());
            }
        }
        if (!parent.InRange)
        {
            parent.ChangeState(new EvadeState());
        }
        else if (!parent.CanSeePlayer())
        {
            parent.ChangeState(new PathState());
        }
    }   
}
