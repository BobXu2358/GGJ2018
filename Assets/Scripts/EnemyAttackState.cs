using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private Enemy _enemy;

    private float tempCount = 0.0f;

    public EnemyAttackState(Enemy enemy)
    {
        _enemy = enemy;
    }

    public void Update()
    {

    }

    public void HandleInput()
    {
        Shoot();

        if (!_enemy.FoundPlayer())
        {
            _enemy.SetEnemyState(new EnemyStandingState(_enemy));
        }
    }

    /// <summary>
    /// 发射子弹
    /// </summary>
    private void Shoot()
    {
        if  (tempCount == 0.0f)
        {
            _enemy.trigger_Shoot = true;
            tempCount += Time.deltaTime;
        }
        else if (tempCount < _enemy.time_AtkInterval)
        {
            tempCount += Time.deltaTime;
        }
        else
        {
            tempCount = 0.0f;
        }
    }
}
