using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletNormal : MonoBehaviour {

    public float bulletLifeTime = 10;

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

        if (ooo.tag == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            if (ooo.tag != "Enemy")
            {
                Destroy(this.gameObject);
            }
        }
    }
}
