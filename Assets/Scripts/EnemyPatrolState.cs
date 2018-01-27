using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    private Enemy _enemy;

    // 通用，判断左右，目标点
    private int isBack = 0;
    private bool isToLeft = false;
    private Vector3 tartgetPos = Vector3.zero;

    //闪烁专用
    private float FadeTime = 0.3f;
    private float ShowTime = 0.3f;
    private Color tempColor = Color.white;
    private float alphaChange = Time.deltaTime / 0.3f;

    public EnemyPatrolState(Enemy enemy)
    {
        _enemy = enemy;

        if (_enemy._Animator != null)
        {
            //_enemy._Animator.SetTrigger("toPatrol");
        }
    }

    public void Update()
    {

    }

    public void HandleInput()
    {
        if (_enemy.FoundPlayer())
        {
            if (_enemy._Type != EnemyType.Flash)
            {
                _enemy.SetEnemyState(new EnemyAlertState(_enemy));
            }
        }

        NavToTartget();
    }

    /// <summary>
    /// 寻路到目标点
    /// </summary>
    private void NavToTartget()
    {
        JudgePatrolPos();

        //根据怪物不同类型进行行为模式的变化 
        if (_enemy._Type == EnemyType.Accelerate || _enemy._Type == EnemyType.Shoot || _enemy._Type == EnemyType.Pierce)
        {
            Vector2 forceDir = tartgetPos - _enemy._selfObj.transform.position;
            forceDir.y = 0.0f;


            if (forceDir.magnitude < 0.05f)
            {
                _enemy._selfObj.transform.position = tartgetPos;
                _enemy.SetEnemyState(new EnemyStandingState(_enemy));
            }
            else
            {
                _enemy._selfObj.GetComponent<Rigidbody2D>().AddForce(forceDir * 1.0f);
                Vector3 moveSpeed = _enemy.speed_run * Vector3.left * Time.deltaTime;
                if (isToLeft) { _enemy._selfObj.transform.Translate(moveSpeed); }
                else { _enemy._selfObj.transform.Translate(-moveSpeed); }
            }
        }
        else if (_enemy._Type == EnemyType.Flash)
        {
            FlashToOnePoint();
        }
    }

    /// <summary>
    /// 判断是否巡逻或回去
    /// </summary>
    private void JudgePatrolPos()
    {
        if (isBack == 0)
        {
            Vector3 offsetBack = _enemy.initPos - _enemy._selfObj.transform.position;
            Vector3 offsetPatrol = _enemy.PatrolTargetPos - _enemy._selfObj.transform.position;

            if (offsetBack.magnitude >= offsetPatrol.magnitude)
            {
                isBack = 1;
                tartgetPos = _enemy.initPos;
            }
            else
            {
                isBack = -1;
                tartgetPos = _enemy.PatrolTargetPos;
                
            }

            tartgetPos.y = _enemy._selfObj.transform.position.y;
            if (tartgetPos.x > _enemy._selfObj.transform.position.x)
            { isToLeft = false;}
            else
            { isToLeft = true;}
        }
        else
        {
            return;
        }
    }

    /// <summary>
    /// 闪烁到某一点
    /// </summary>
    private void FlashToOnePoint()
    {
        //alphaChange = Time.deltaTime /0.5f;
        if (FadeTime > 0)
        {
            FadeTime -= Time.deltaTime; 
            tempColor.a -= alphaChange;
            tempColor.a = Mathf.Clamp01(tempColor.a);
            _enemy._selfObj.GetComponent<Renderer>().material.SetColor("_Color", tempColor);
            //Debug.Log(tempColor.a);
        }
        else
        {
            _enemy._selfObj.transform.position = tartgetPos;

            if (ShowTime > 0)
            {
                ShowTime -= Time.deltaTime;
                tempColor.a += alphaChange;
                tempColor.a = Mathf.Clamp01(tempColor.a);
                _enemy._selfObj.GetComponent<Renderer>().material.SetColor("_Color", tempColor);
            }
            else
            {
                _enemy._selfObj.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                _enemy.SetEnemyState(new EnemyStandingState(_enemy));
            }
        }
    }
}
