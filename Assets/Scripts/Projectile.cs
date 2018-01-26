using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float speed;

    void Start()
    {
        
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            Destroy(col.gameObject);
            this.gameObject.GetComponent<PlayerAction>().enabled = false;
            col.gameObject.GetComponent<PlayerAction>().enabled = true;
        }
    }
}
