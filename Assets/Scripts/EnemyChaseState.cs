using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyBaseState {

    private Enemy _enemy;

    private float tempTime;
    public EnemyChaseState(Enemy enemy)
    {
        _enemy = enemy;

        if (_enemy._Animator != null)
        {
            _enemy._Animator.SetTrigger("toChase");
        }
    }

    public void Update()
    {

    }


    public void HandleInput()
    {
        ChaseTarget();

        ////目标玩家消失在视野里，则继续追踪玩家3s，若仍不可见，则变为puzzle状态
        //if (_enemy.ViewBlocked())


        if (! _enemy.FoundPlayer())
        {
            _enemy.SetEnemyState(new EnemyStandingState(_enemy));
        }
    }

    /// <summary>
    /// 追逐目标
    /// </summary>
    public void ChaseTarget()
    {
        //Debug.Log("Chase");
        Vector2 forceDir = _enemy.Player.transform.position - _enemy._selfObj.transform.position;
        if (_enemy._Type != CharacterType.Fly)
        {
            forceDir.y = 0.0f;
        }

        forceDir.Normalize();
        //Debug.Log(forceDir);
        // 移动
        Vector3 moveSpeed = _enemy.speed_run * forceDir * Time.deltaTime;
        _enemy._selfObj.transform.Translate(moveSpeed);
        if (forceDir.x < 0.0f)//Left
        {
            Vector3 scale = _enemy._selfObj.transform.localScale;
            scale.x = -1f;
            _enemy._selfObj.transform.localScale = scale;
        }
        else //right
        {
            Vector3 scale = _enemy._selfObj.transform.localScale;
            scale.x = 1f;
            _enemy._selfObj.transform.localScale = scale;
        }

        // 判定是否转换状态
        if (_enemy._Type == CharacterType.Accelerate)
        {
        }
        else if (_enemy._Type == CharacterType.Shoot  || _enemy._Type == CharacterType.Pierce)
        {
            //if (forceDir.magnitude < _enemy.attackDistance)
            //{
                _enemy.SetEnemyState(new EnemyAttackState(_enemy));
            //}
        }
    }
}
