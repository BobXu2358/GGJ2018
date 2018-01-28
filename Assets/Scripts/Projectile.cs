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

            //camera stuff
            StartCoroutine(CameraLerp(col.gameObject));
            Camera.main.GetComponent<CameraManager>().followingTf = col.gameObject.transform;
            Camera.main.GetComponent<CameraManager>().followingBc = (CapsuleCollider2D)col;

            //disable player control script
            player.GetComponent<PlayerAction>().enabled = false;
            player.GetComponent<EnemyAI>().enabled = true;
            player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            player.GetComponent<Animator>().SetBool("isMoving", false);
            player.GetComponent<PlayerAction>().PlayTransmitSuccessSound();

            //change player tag
            player.tag = "Enemy";

            //enable target's player control script
            col.gameObject.GetComponent<PlayerAction>().enabled = true;
            col.gameObject.GetComponent<EnemyAI>().enabled = false;
            col.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.white);

            //change the tag of new player
            col.gameObject.tag = "Player";

            //camera stuff
            Camera.main.GetComponent<CameraManager>().followingTf = col.gameObject.transform;
            Camera.main.GetComponent<CameraManager>().followingBc = (CapsuleCollider2D)col;

            GetBaseAttribute(col.gameObject);
            ResetTargetPlayer();
        }
    }

    private IEnumerator CameraLerp(GameObject enemy)
    {
        float counts = 200f;
        float para = 0.0f;
        while(counts > 0)
        {
            counts--;
            para += 0.005f;
            Camera.main.transform.position = Vector2.Lerp(player.transform.position, enemy.transform.position, para);
            yield return new WaitForSeconds(0.01f);
        }
        
    }

    /// <summary>
    /// EnemyAI内的基本属性赋予给PlayerAction中的属性
    /// </summary>
    /// <param name="targetObj"></param>
    private void GetBaseAttribute(GameObject targetObj)
    {
        targetObj.GetComponent<PlayerAction>().playerType = targetObj.GetComponent<EnemyAI>().m_Type;
        targetObj.GetComponent<PlayerAction>().moveSpeed = targetObj.GetComponent<EnemyAI>().SpeedRun;
    }

    /// <summary>
    /// 重新设置敌人脚本中的玩家
    /// </summary>
    void ResetTargetPlayer()
    {
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject em in enemyList)
        {
            em.GetComponent<EnemyAI>().SetPlayer();
        }
    }
}
