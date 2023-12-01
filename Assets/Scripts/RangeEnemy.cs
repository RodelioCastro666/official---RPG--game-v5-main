using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : Enemy
{
    [SerializeField]
    private GameObject arrowPrefab;

    [SerializeField]
    private Transform[] exitPoints;

    private bool updateDirection = false;
   
    private float fieldOfview = 120;

    protected override void Update()
    {
        LookAtTarget();
        base.Update();
    }

    public void Shoot(int exitIndex)
    {
        SpellScript s = Instantiate(arrowPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();

        s.Initialize(MyTarget.MyHitBox, damage, this);
    }

    private void UpdateDirection()
    {

    }

    private void LookAtTarget()
    {
        if(MyTarget != null)
        {
            Vector2 directionToTarget = (MyTarget.transform.position - transform.position).normalized;

            Vector2 facing = new Vector2(MyAnimator.GetFloat("x"), MyAnimator.GetFloat("y"));

            float angleToTarget = Vector2.Angle(facing, directionToTarget);

            if(angleToTarget > fieldOfview / 2)
            {
                MyAnimator.SetFloat("x", directionToTarget.x);
                MyAnimator.SetFloat("y", directionToTarget.y);
            }
        }
    }
}
