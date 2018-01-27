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

            GameObject enemy = col.gameObject;

            //destory the bullet
            Destroy(gameObject);

            //disable player control script
            player.GetComponent<PlayerAction>().enabled = false;
            player.GetComponent<MainController>().enabled = false;
            player.GetComponent<Fire>().enabled = false;
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

            //change player tag
            player.tag = "Enemy";

            //enable target's player control script

            enemy.GetComponent<PlayerAction>().enabled = true;
            enemy.GetComponent<Fire>().enabled = true;
            enemy.GetComponent<MainController>().enabled = true;

            //change the tag of new player
            enemy.tag = "Player";

            //camera stuff
            Camera.main.GetComponent<CameraManager>().followingTf = enemy.transform;
            Camera.main.GetComponent<CameraManager>().followingBc = (BoxCollider2D) col;
        }
    }
}
