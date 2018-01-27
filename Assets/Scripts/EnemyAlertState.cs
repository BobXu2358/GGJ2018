using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlertState : EnemyBaseState {

    private Enemy _enemy;

    private float counts_Alert;   //警惕到追逐的计数
    private float counts_Back;    //警惕到回复的计数

    public EnemyAlertState (Enemy enemy)
    {
        _enemy = enemy;
        counts_Alert = 0;
        counts_Back = 0;

        //if (_enemy._Animator != null)
        //{
        //    _enemy._Animator.SetTrigger("toAlert");
        //}
    }

	public void Update () {
		
	}

    public void HandleInput()
    {
        SwitchingState();
    }


    /// <summary>
    /// 等待状态切换
    /// </summary>
    private void SwitchingState()
    {
        if (_enemy.FoundPlayer())
        {
            counts_Back = 0;
            if (counts_Alert <= _enemy.time_Alert)
            {
                counts_Alert += Time.deltaTime;
            }
            else
            {
                _enemy.SetEnemyState(new EnemyChaseState(_enemy));
            }
        }
        else
        {
            if (counts_Alert > 0)
            {
                counts_Alert -= Time.deltaTime;
            }

            if (counts_Back <= _enemy.time_Alert)
            {
                counts_Back += Time.deltaTime;
            }
            else
            {
                _enemy.SetEnemyState(new EnemyStandingState(_enemy));
            }
        }
    }
}
