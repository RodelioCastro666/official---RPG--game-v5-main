using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathState : IState
{
   

    private Vector3 destination;

    private Vector3 current;

    private Vector3 goal;

    private Transform transform;

    private Enemy parent;

    private Vector3 targetPos;

    public void Enter(Enemy parent)
    {
        this.parent = parent;

        this.transform = parent.transform.parent;

        targetPos = Player.MyInstance.MyCurrenTile.position;

        if(targetPos != parent.MyCurrenTile.position)
        {
            parent.MyPath = parent.MyAstar.Algorithm(parent.MyCurrenTile.position, targetPos);
        }
        if(parent.MyPath != null)
        {
            current = parent.MyPath.Pop();
            destination = parent.MyPath.Pop();
            this.goal = parent.MyCurrenTile.position;
        }
        else
        {
            parent.ChangeState(new EvadeState());
        }

       
    }

    public void Exit()
    {
        parent.MyPath = null;
    }

    public void Update()
    {
        if(parent.MyPath != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, 2 * Time.deltaTime);

            parent.ActivateLayer("Walk");

            Vector3Int dest = parent.MyAstar.MyTilemap.WorldToCell(destination); 
            Vector3Int cur = parent.MyAstar.MyTilemap.WorldToCell(current);

            float distance = Vector2.Distance(destination, transform.position);

            float totalDistance = Vector2.Distance(parent.MyTarget.transform.parent.position, transform.position);

            if (cur.y > dest.y)
            {
                parent.Direction = Vector2.down;
            }
            else if (cur.y < dest.y)
            {
                parent.Direction = Vector2.up;
            }
            if (cur.y == dest.y)
            {
                if (cur.x > dest.x)
                {
                    parent.Direction = Vector2.left;
                }
                else if (cur.x < dest.x)
                {
                    parent.Direction = Vector2.right;
                }
            }

            if(totalDistance <= parent.MyAttackRange)
            {
                parent.ChangeState(new AttackState()); 
            }

            else if(Player.MyInstance.MyCurrenTile.position == parent.MyCurrenTile.position)
            {
                parent.ChangeState(new FollowState());
            }
            if (distance <= 0f)
            {
                if(parent.MyPath.Count > 0)
                {
                    current = destination;
                    destination = parent.MyPath.Pop();

                    if(targetPos != Player.MyInstance.MyCurrenTile.position)
                    {
                        parent.ChangeState(new PathState());
                    }

                }
                else
                {
                    parent.MyPath = null;
                    parent.ChangeState(new PathState());
                }
            }
        }
       
    }
}
