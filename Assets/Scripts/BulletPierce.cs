using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletPierce : MonoBehaviour {

    public float bulletLifeTime = 5;

    // Use this for initialization
    void Start () {
        Destroy(this.gameObject, bulletLifeTime);
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject ooo = collision.gameObject;
        if(collision.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }
    }
}
