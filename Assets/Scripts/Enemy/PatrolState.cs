using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : EnemyBaseState
{
    public override void EnterState(Enemy enemy)
    {
        enemy.animState = 0;
        enemy.SwitchPoint();
    }

    public override void OnUpdate(Enemy enemy)
    {
        if(!enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            enemy.animState = 1;
            enemy.MoveToTarget();
        } 

        if (Mathf.Abs(enemy.transform.position.x - enemy.targetPoint.position.x) < 0.1f)
        {
            enemy.TransitionToState(enemy.patrolState);
        }
    }
}
