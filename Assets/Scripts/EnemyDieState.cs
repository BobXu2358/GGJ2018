using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDieState : EnemyBaseState
{
    private Enemy _enemy;

    public EnemyDieState(Enemy enemy)
    {
        _enemy = enemy;
        if (_enemy._Animator != null)
        {
            _enemy._Animator.SetTrigger("toDie");
        }
        //_enemy.agent.isStopped = true;
    }

    public void Update()
    {

    }

    public void HandleInput()
    {
    }
}
