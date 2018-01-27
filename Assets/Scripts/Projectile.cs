using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float bulletLifeTime;
    private GameObject player;

    void Start()
    {
        Destroy(gameObject, bulletLifeTime);
        //find player object
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            //destory the bullet
            Destroy(gameObject);

            //disable player control script
            player.GetComponent<PlayerAction>().enabled = false;

            //change player tag
            player.tag = "Enemy";

            //enable target's player control script
            col.gameObject.GetComponent<PlayerAction>().enabled = true;

            //change the tag of new player
            col.gameObject.tag = "Player";

            //camera stuff
            Camera.main.GetComponent<CameraManager>().followingTf = col.gameObject.transform;
            Camera.main.GetComponent<CameraManager>().followingBc = (BoxCollider2D)col;

            ResetTargetPlayer();
        }
    }

    /// <summary>
    /// 重新设置敌人脚本中的玩家
    /// </summary>
    void ResetTargetPlayer()
    {
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject em in enemyList)
        {
            //em.GetComponent<EnemyAI>().SetPlayer();
        }
    }
}
