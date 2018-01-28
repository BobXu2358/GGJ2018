using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchButton : MonoBehaviour {


    public GameObject Object_Generate;
    public Transform Pos_Generate;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject ooo = collision.gameObject;
        
        if (ooo.tag != "Obstacle")
        {
            Debug.Log(1111111);
            Vector3 pos = Pos_Generate.position;
            pos.z = 0.0f;
            Instantiate(Object_Generate, pos, Quaternion.identity);
        }
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		GameObject ooo = collision.gameObject;

		if (ooo.tag != "Obstacle")
		{
			Debug.Log(1111111);
			Vector3 pos = Pos_Generate.position;
			pos.z = 0.0f;
			Instantiate(Object_Generate, pos, Quaternion.identity);
		}
	}
}
