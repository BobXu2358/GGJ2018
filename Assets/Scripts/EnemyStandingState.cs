using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStandingState : EnemyBaseState {

    private Enemy _enemy;

    private float counts_ToPatrol = 0;  //= Random.Range(3,10);

    public EnemyStandingState(Enemy enemy)
    {
        _enemy = enemy;

        if (_enemy._Animator != null)
        {
            _enemy._Animator.SetBool("isMoving",false);
        }
    }

    public void Update()
    {

    }

    public void HandleInput()
    {
        //if (_enemy.FoundPlayer())
        //{
        //    if (_enemy._Type != CharacterType.Flash)
        //    {
        //        _enemy.SetEnemyState(new EnemyAlertState(_enemy));
        //    } 
        //}

        if (_enemy._Type == CharacterType.Cast)
        {
            if (_enemy.FoundPlayer())
            {
                _enemy.SetEnemyState(new EnemyAlertState(_enemy));
            }
        }

        if (Condition_SwitchToPatrolState())
        {
            _enemy.SetEnemyState(new EnemyPatrolState(_enemy));
        }
    }

    /// <summary>
    /// 转化为巡逻状态
    /// </summary>
    /// <returns></returns>
    private bool Condition_SwitchToPatrolState()
    {
        bool result = false;

        if (counts_ToPatrol < _enemy.time_Patrol)
        {
            counts_ToPatrol += Time.deltaTime;
        }
        else
        {
            result = true;
        }

        return result;
    }
}
