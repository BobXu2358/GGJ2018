﻿using System.Collections;
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
            player.GetComponent<Fire>().enabled = false;

            //change player tag
            player.tag = "Enemy";

            //enable target's player control script
            col.gameObject.GetComponent<PlayerAction>().enabled = true;
            col.gameObject.GetComponent<Fire>().enabled = true;

            //change the tag of new player
            col.gameObject.tag = "Player";
        }
    }
}
