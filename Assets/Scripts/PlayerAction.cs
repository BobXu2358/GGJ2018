using UnityEngine;

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
    public bool success = false;

    void FixedUpdate() {
        if(playerType != CharacterType.None){
            //Control player to move horizontally
            Vector2 playerSpeed = playerRb.velocity;
            playerSpeed.x = Input.GetAxis("Horizontal") * moveSpeed;
            playerRb.velocity = playerSpeed;

            if (grounded)
            {
                if (playerRb.velocity.magnitude >= 0.1f)
                {
                    this.GetComponent<Animator>().SetBool("isMoving",true);
                }
                else
                {
                    this.GetComponent<Animator>().SetBool("isMoving", false);
                }
            }
            


            //Control player to jump
            if(grounded && Input.GetButton("Jump")){
                grounded = false;
                playerRb.AddForce(new Vector2(0, jumpForce));
            }
        }

        //Make player not move outer of map border
        Vector3 finPlayerPos = playerTf.position;
        if(finPlayerPos.x - playerBc.size.x / 2 <= mapGroundTf.position.x - mapGroundBc.size.x / 2)
            finPlayerPos.x = mapGroundTf.position.x - mapGroundBc.size.x / 2 + playerBc.size.x / 2;
        if(finPlayerPos.x + playerBc.size.x / 2 >= mapGroundTf.position.x + mapGroundBc.size.x / 2)
            finPlayerPos.x = mapGroundTf.position.x + mapGroundBc.size.x / 2 - playerBc.size.x / 2;
        playerTf.position = finPlayerPos;

        //Make player turn back when changing direction
        Vector3 finPlayerScale = playerTf.localScale;
        if(Input.GetAxisRaw("Horizontal") * finPlayerScale.x < 0) finPlayerScale.x *= -1;
        playerTf.localScale = finPlayerScale;

        //Make player fire the brain wave
        if (Input.GetButtonDown("Fire1")) {

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            Vector2 dir = mousePos - new Vector2(fireOffset.position.x, fireOffset.position.y);

            //instantiate bullet
            GameObject bullet = Instantiate(Projectile, fireOffset.position + new Vector3(0, 0, -5), fireOffset.rotation);
            dir.Normalize();
            //let it go
            bullet.GetComponent<Rigidbody2D>().velocity = dir * fireSpeed;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if(playerType == CharacterType.Shoot)
            {
                Shoot(playerTf.localScale.x);
            }
            if(playerType == CharacterType.Pierce)
            {
                Pierce(playerTf.localScale.x);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collisionObject){
        if(collisionObject.gameObject.tag == "Enemy" || collisionObject.gameObject.name == "Map Ground" || collisionObject.gameObject.tag == "Bullet"){
            Debug.Log(collisionObject.gameObject.tag);
            alive = false;
        }
        if(collisionObject.gameObject.name == "End")
            success = true;
    }

    void OnCollisionStay2D(Collision2D collisionObject){
        //Check if the player is on the ground to jump
        //if(playerTf.position.y - playerBc.size.y / 2 >= collisionObject.gameObject.transform.position.y + collisionObject.gameObject.GetComponent<BoxCollider2D>().size.y / 2){
        if(collisionObject.gameObject.tag == "Obstacle")
            grounded = true;
        //}
    }

    void Shoot(float facing)
    {
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
            bullet0.GetComponent<Rigidbody2D>().velocity = -transform.right * fireSpeed;
            bullet1.GetComponent<Rigidbody2D>().velocity = -dir1 * fireSpeed;
            bullet2.GetComponent<Rigidbody2D>().velocity = -dir2 * fireSpeed;
        }
    }

    void Pierce(float facing)
    {
        GameObject bullet0 = Instantiate(PiercingBullet, fireOffset.position, fireOffset.rotation);
        //let it go
        if (facing == 1)
            bullet0.GetComponent<Rigidbody2D>().velocity = transform.right * fireSpeed;
        if (facing == -1)
            bullet0.GetComponent<Rigidbody2D>().velocity = -transform.right * fireSpeed;
    }

}
