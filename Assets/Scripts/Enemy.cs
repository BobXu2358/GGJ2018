using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy {
    EnemyBaseState _state;

    //自身属性字段
    public GameObject _selfObj;
    public Animator _Animator;
    public CharacterType _Type;

    //附加属性
    public float time_Patrol;
    public float time_Alert;            // 警惕转换为追踪的时间
    public float dis_Find;              // 发现玩家的距离
    public bool trigger_Shoot;          // 射击的触发

    public float speed_run;             // 奔跑的速度
    public float time_AtkInterval;      // 怪物的攻击间隔

    //寻路字段
    Vector3 NavDestination; 
    //public NavMeshAgent agent;
    public Vector3 initPos;
    public Vector3 PatrolTargetPos;
    public GameObject Player;

    public Enemy()
    {
        _state = new EnemyStandingState(this);
    }

    public void SetEnemyState(EnemyBaseState newState)
    {
        _state = newState;
    }

    public void Update()
    {
        _state.HandleInput();
    }

    ///// <summary>
    ///// 设置寻路目标点
    ///// </summary>
    ///// <param name="trans"></param>
    //public void SetNavTargetPos(Transform trans)
    //{
    //    NavDestination = trans.position;
    //}

    ///// <summary>
    ///// 开始寻路
    ///// </summary>
    //public void StartNav()
    //{
    //    agent.SetDestination(NavDestination);
    //}

    //public void SetInitPosAsNavDes()
    //{
    //    NavDestination = initPos;
    //}

    ///// <summary>
    ///// 设置追逐时寻路参数
    ///// </summary>
    //public void SetNavParaForChase()
    //{
    //    agent.isStopped = false;
    //    agent.stoppingDistance = 0.0f;
    //    agent.speed = speed_run;
    //}

    ///// <summary>
    ///// 设置正常巡逻时的寻路参数
    ///// </summary>
    //public void SetNavParaForNormal()
    //{ 
    //    agent.isStopped = false;
    //    agent.stoppingDistance = 0.0f;
    //    agent.speed = speed_walk;
    //}


    /// <summary>
    /// 寻找玩家
    /// </summary>
    /// <returns></returns>
    public bool FoundPlayer()
    {
        bool found = false;

        if(_Type == CharacterType.None || _Type == CharacterType.Human)
            return false;

        Vector3 offset = Player.transform.position - _selfObj.transform.position;
        float dis = offset.magnitude;
        if (dis <= dis_Find)
        {
            found = true;
            if (_selfObj.transform.localScale.x > 0.0f && offset.x < 0.0f)
            {
                found = false;
            }
            if (_selfObj.transform.localScale.x < 0.0f && offset.x >= 0.0f)
            {
                found = false;
            }
        }
        return found;
    }

    /// <summary>
    /// 视线被遮挡（用于追逐状态）
    /// </summary>
    /// <returns></returns>
    public bool ViewBlocked()
    {
        bool result = false;

        RaycastHit hitt;
        if (Physics.Raycast(_selfObj.transform.position,_selfObj.transform.forward,out hitt,dis_Find))
        {
            if (hitt.transform != null)
            {
                if (hitt.collider.gameObject.tag != "Player")
                {
                    result = true;
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 只有在发现玩家时才作判定
    /// </summary>
    /// <returns></returns>
    private bool ViewBlockedThroughLook()
    {
        bool result = false;

        RaycastHit hitt;
        Vector3 lookRotation = Player.transform.position - _selfObj.transform.position;
        if (Physics.Raycast(_selfObj.transform.position + Vector3.up * 0.2f, lookRotation, out hitt, dis_Find))
        {
            if (hitt.transform != null)
            {
                if (hitt.collider.gameObject.tag != "Player")
                {
                    result = true;
                }
            }
        }

        return result;
    }
}
