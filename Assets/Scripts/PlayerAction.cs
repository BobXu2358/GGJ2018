﻿using UnityEngine;

public class PlayerAction : MonoBehaviour {
    public CharacterType playerType;
    public Transform playerTf;
    public CapsuleCollider2D playerBc;
    public Rigidbody2D playerRb;
    public Transform mapGroundTf;
    public BoxCollider2D mapGroundBc;
    public float moveSpeed;
    public float jumpForce;
    public GameObject Projectile;
    public float fireSpeed;
    public Transform fireOffset;
    bool grounded = true;
    public bool alive = true;
    public GameObject MultiShotBullet;
    public GameObject PiercingBullet;
    public float SprintSpeed;
    public float flashDistance;
    public bool success = false;

    private float realTimeSpeed;
    private GameObject mindBullet;
    private Animator anim;

    private float FadeTime = 0.3f;
    private float ShowTime = 0.3f;
    private Color tempColor = Color.white;
    private float alphaChange;

    void Start()
    {
        realTimeSpeed = moveSpeed;
        anim = GetComponent<Animator>();
        alphaChange = Time.deltaTime / 0.3f;
    }

    void FixedUpdate()
    {
        if (alive)
        {
            if (playerType != CharacterType.None)
            {
                //Control player to move horizontally
                //Debug.Log(realTimeSpeed);
                Vector2 playerSpeed = playerRb.velocity;
                playerSpeed.x = Input.GetAxis("Horizontal") * realTimeSpeed;
                playerRb.velocity = playerSpeed;

                if (grounded)
                {
                    if (Input.GetAxisRaw("Horizontal") != 0 && grounded)
                    {
                        this.GetComponent<Animator>().SetBool("isMoving", true);
                    }
                    else
                    {
                        this.GetComponent<Animator>().SetBool("isMoving", false);
                    }
                }

                //Control player to jump
                if (grounded && Input.GetButton("Jump"))
                {
                    grounded = false;
                    playerRb.AddForce(new Vector2(0, jumpForce));
                }
            }

            //Make player not move outer of map border
            Vector3 finPlayerPos = playerTf.position;
            if (finPlayerPos.x - playerBc.size.x / 2 <= mapGroundTf.position.x - mapGroundBc.size.x / 2)
                finPlayerPos.x = mapGroundTf.position.x - mapGroundBc.size.x / 2 + playerBc.size.x / 2;
            if (finPlayerPos.x + playerBc.size.x / 2 >= mapGroundTf.position.x + mapGroundBc.size.x / 2)
                finPlayerPos.x = mapGroundTf.position.x + mapGroundBc.size.x / 2 - playerBc.size.x / 2;
            playerTf.position = finPlayerPos;

            //Make player turn back when changing direction
            Vector3 finPlayerScale = playerTf.localScale;
            if (Input.GetAxisRaw("Horizontal") * finPlayerScale.x < 0) finPlayerScale.x *= -1;
            playerTf.localScale = finPlayerScale;

            //Make player fire the brain wave
            if (Input.GetButtonDown("Fire") && mindBullet == null)
            {

                Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                Vector2 dir = mousePos - new Vector2(fireOffset.position.x, fireOffset.position.y);

                //instantiate bullet
                mindBullet = Instantiate(Projectile, fireOffset.position + new Vector3(0, 0, -5), fireOffset.rotation);
                dir.Normalize();
                //let it go
                mindBullet.GetComponent<Rigidbody2D>().velocity = dir * fireSpeed;
            }

            float fallMultiplier = 2.5f;
            float lowJumpMultiplier = 2f;
            if(playerRb.velocity.y < 0){
                playerRb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }else if(playerRb.velocity.y > 0 && !Input.GetButton("Jump")){
                playerRb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }

            if (playerType == CharacterType.Accelerate)
            {
                if (Input.GetButton("Power"))
                {
                    realTimeSpeed = SprintSpeed;
                    Sprint();
                }
                else
                {
                    realTimeSpeed = moveSpeed;
                    anim.SetTrigger("exit");
                }

            }

            else if (Input.GetButtonDown("Power"))
            {
                if (playerType == CharacterType.Shoot)
                    Shoot(playerTf.localScale.x);
                if (playerType == CharacterType.Pierce)
                    Pierce(playerTf.localScale.x);
                if (playerType == CharacterType.Accelerate)
                {
                    realTimeSpeed = SprintSpeed;
                    Sprint();
                }
                if (playerType == CharacterType.Flash)
                    Flash(playerTf.localScale.x);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collisionObject){
        if(collisionObject.gameObject.tag == "Enemy" || collisionObject.gameObject.name == "Map Ground" || collisionObject.gameObject.tag == "Bullet"){
            alive = false;
        }
        if(collisionObject.gameObject.name == "End")
            success = true;
    }

    void OnCollisionStay2D(Collision2D collisionObject){
        //Check if the player is on the ground to jump
        //if(playerTf.position.y - playerBc.size.y / 2 >= collisionObject.gameObject.transform.position.y + collisionObject.gameObject.GetComponent<BoxCollider2D>().size.y / 2){
        if(collisionObject.gameObject.tag == "Obstacle"){
            Vector3 tmpPoint = playerTf.position + new Vector3(-playerBc.size.x / 3, -playerBc.size.y / 2, 0);
            RaycastHit2D hit = Physics2D.Raycast(tmpPoint, new Vector2(0, -1));
            Debug.DrawLine(tmpPoint, hit.point);
            if(hit.collider != null){
                if(hit.collider.gameObject.tag == "Obstacle" && Mathf.Abs(tmpPoint.y - hit.point.y) <= 0.05f){
                    grounded = true;
                }
            }
            tmpPoint = playerTf.position + new Vector3(playerBc.size.x / 3, -playerBc.size.y / 2, 0);
            hit = Physics2D.Raycast(tmpPoint, new Vector2(0, -1));
            if(hit.collider != null){
                if(hit.collider.gameObject.tag == "Obstacle" && Mathf.Abs(tmpPoint.y - hit.point.y) <= 0.05f){
                    grounded = true;
                }
            }
        }
        //}
    }

    void PlayJumpSound()
    {
        ;
    }

    void PlayMoveSound()
    {
        ;
    }

    void PlayTransmitSound()
    {
        ;
    }

    void Shoot(float facing)
    {
        Debug.Log("shooting");
        anim.SetBool("shoot", true);
        //let it go
        GameObject bullet0 = Instantiate(MultiShotBullet, fireOffset.position, fireOffset.rotation);
        GameObject bullet1 = Instantiate(MultiShotBullet, fireOffset.position, fireOffset.rotation);
        GameObject bullet2 = Instantiate(MultiShotBullet, fireOffset.position, fireOffset.rotation);

        Vector2 dir1 = Quaternion.Euler(0, 0, 45) * Vector2.right;
        Vector2 dir2 = Quaternion.Euler(0, 0, -45) * Vector2.right;
        //let it go
        if (facing == 1)
        {
            bullet0.GetComponent<Rigidbody2D>().velocity = transform.right * fireSpeed;
            bullet1.GetComponent<Rigidbody2D>().velocity = dir1 * fireSpeed;
            bullet2.GetComponent<Rigidbody2D>().velocity = dir2 * fireSpeed;
        }

        if (facing == -1)
        {
            bullet0.GetComponent<SpriteRenderer>().flipX = true;
            bullet1.GetComponent<SpriteRenderer>().flipX = true;
            bullet2.GetComponent<SpriteRenderer>().flipX = true;

            bullet0.GetComponent<Rigidbody2D>().velocity = -transform.right * fireSpeed;
            bullet1.GetComponent<Rigidbody2D>().velocity = -dir1 * fireSpeed;
            bullet2.GetComponent<Rigidbody2D>().velocity = -dir2 * fireSpeed;
        }
    }

    void Pierce(float facing)
    {
        anim.SetTrigger("shoot");
        GameObject bullet = Instantiate(PiercingBullet, fireOffset.position, fireOffset.rotation);
        //let it go
        if (facing == 1)
            bullet.GetComponent<Rigidbody2D>().velocity = transform.right * fireSpeed;
        if (facing == -1)
        {
            bullet.GetComponent<SpriteRenderer>().flipX = true;
            bullet.GetComponent<Rigidbody2D>().velocity = -transform.right * fireSpeed;
        }
            
    }

    void Sprint()
    {
        anim.SetTrigger("start");
        realTimeSpeed = SprintSpeed;
    }

    void Flash(float facing)
    {

        anim.SetTrigger("flash");

        float x = transform.position.x;


        if (facing == 1)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward);
            if(hit.collider!= null && hit.distance > flashDistance) 
                transform.position = new Vector3(x + flashDistance, transform.position.y, transform.position.z);
            else
                transform.position = new Vector3(x + flashDistance, transform.position.y, transform.position.z);
        }

        else
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.forward);
            if (hit.collider != null && hit.distance > flashDistance)
                transform.position = new Vector3(x - flashDistance, transform.position.y, transform.position.z);
            else
                transform.position = new Vector3(x + flashDistance, transform.position.y, transform.position.z);
        }

    }
}
