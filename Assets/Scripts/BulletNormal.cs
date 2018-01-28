using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletNormal : MonoBehaviour {

    public float bulletLifeTime = 10;

    public GameObject spark;

    public Transform head;

    // Use this for initialization
    void Start () {
        Destroy(this.gameObject, bulletLifeTime);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 碰撞
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject ooo = collision.gameObject;

        Debug.Log(ooo.name);

        if (ooo.tag == "Player")
        {
            ooo.GetComponent<PlayerAction>().alive = false;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            if (ooo.tag == "Obstacle")
            {
                Instantiate(spark,head.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
        }
    }
}
