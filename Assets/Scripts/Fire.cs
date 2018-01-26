using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

    public GameObject Projectile;
    public float speed;
    public Transform offset;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
            shoot();
    }

    void shoot()
    {

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        Vector2 dir = mousePos - new Vector2(offset.position.x, offset.position.y);

        //instantiate bullet
        GameObject bullet = Instantiate(Projectile, offset.position, offset.rotation);
        //let it go
        bullet.GetComponent<Rigidbody2D>().velocity = dir * speed;
    }
}
