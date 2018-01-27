﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class testEnemy : MonoBehaviour {

    [HideInInspector] public Transform PatrolTarget;
    [HideInInspector] public GameObject Player;
    public float TimeToPatrol = 3.0f;    //待机到巡逻的时间
    public float TimeAlert = 0.0f;       //警戒到追逐的时间
    public float DistanceFind = 10.0f;   //发现玩家的距离
    public float SpeedRun = 2.0f;        //移动速度
    public float AttackInterval = 2.0f;  //发射子弹的间隔
    public EnemyType m_Type;             //怪物类型
    public GameObject Prefab_Bullet;     //子弹的预制体

    
    private Enemy _enemy;
    [HideInInspector] public AudioSource m_AudioSource;
    [HideInInspector] public Animator m_Animator;
    [HideInInspector] private AudioClip m_Sound_Collide;
    [HideInInspector] private AudioClip m_Sound_Breath;

    //public float TimeChaseExtend;         //视野遮挡后，继续沿着原路走的时间
    // Use this for initialization
    void Awake () {
        _enemy = new Enemy();

        PatrolTarget = transform.Find("PatrolTartget");
        _enemy._Type = m_Type;
        _enemy.PatrolTargetPos = PatrolTarget.position;
        _enemy.initPos = this.transform.position;
        _enemy.Player = this.Player;
        _enemy._selfObj = this.gameObject;

        _enemy.time_Patrol = TimeToPatrol;
        _enemy.time_Alert = TimeAlert;
        _enemy.dis_Find = DistanceFind;
        _enemy.speed_run = SpeedRun;
        _enemy.time_AtkInterval = AttackInterval;
        _enemy.trigger_Shoot = false;

        m_AudioSource = this.GetComponent<AudioSource>();
        m_Animator = this.GetComponent<Animator>();
        _enemy._Animator = m_Animator;

        Player = GameObject.FindWithTag("Player");
        if (Player != null)
        {
            _enemy.Player = Player;
        }
        else
        {
            Debug.LogError("找不到tag为Player的物体！");
        }
    }
	
	// Update is called once per frame
	void Update () {
        _enemy.Update();
        Shoot();
	}

    private void OnCollisionEnter(Collision collision)
    {
        GameObject ooo = collision.gameObject;

        if (ooo.layer == LayerMask.NameToLayer("Object"))
        {
            //PlayBumpAudio();
        }

        if (ooo.tag == "Player")
        {
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        GameObject ooo = collision.gameObject;
        if (ooo.tag == "Door")
        {
            //ContactPoint con = collision.contacts[0];
            //ooo.GetComponent<Rigidbody>().AddForceAtPosition(this.transform.forward * 1.0f, con.point, ForceMode.Force);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
    }

    /// <summary>
    /// 射击
    /// </summary>
    private void Shoot()
    {
        if (_enemy.trigger_Shoot)
        {
            Instantiate(Prefab_Bullet, this.transform.position, Quaternion.identity);
            _enemy.trigger_Shoot = false;
        }
    }

    /// <summary>
    /// 播放撞击的音效
    /// </summary>
    private void PlayBumpAudio()
    {
        m_AudioSource.clip = m_Sound_Collide;
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
    }

    /// <summary>
    /// 播放呼吸
    /// </summary>
    private void PlayBreatheAudio()
    {
        m_AudioSource.clip = m_Sound_Breath;
        m_AudioSource.Play();
    }

}