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
    [HideInInspector] public bool alive = true;
    [HideInInspector] public bool success = false;

    void FixedUpdate(){
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
        if (Input.GetButtonDown("Fire")){
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            Vector2 dir = mousePos - new Vector2(fireOffset.position.x, fireOffset.position.y);

            //instantiate bullet
            GameObject bullet = Instantiate(Projectile, fireOffset.position + new Vector3(0, 0, -5), fireOffset.rotation);
            dir.Normalize();
            //let it go
            bullet.GetComponent<Rigidbody2D>().velocity = dir * fireSpeed;
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
}
