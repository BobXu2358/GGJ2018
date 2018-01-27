using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAction : MonoBehaviour {
    public CharacterType playerType;
    public Transform playerTf;
    public BoxCollider2D playerBc;
    public Rigidbody2D playerRb;
    public Transform mapGroundTf;
    public BoxCollider2D mapGroundBc;
    public float moveSpeed;
    public float jumpForce;
    public GameObject Projectile;
    public float fireSpeed;
    public Transform fireOffset;
    bool grounded = true;

    void FixedUpdate(){
        if(playerType != CharacterType.None){
            //Control player to move horizontally
            Vector2 playerSpeed = playerRb.velocity;
            playerSpeed.x = Input.GetAxis("Horizontal") * moveSpeed;
            playerRb.velocity = playerSpeed;

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

        //Make level restart when player fall down and die
        if(playerTf.position.y - playerBc.size.y / 2 <= mapGroundTf.position.y + mapGroundBc.size.y / 2)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        //Make player turn back when changing direction
        Vector3 finPlayerScale = playerTf.localScale;
        if(Input.GetAxisRaw("Horizontal") == 1) finPlayerScale.x = 1;
        if(Input.GetAxisRaw("Horizontal") == -1) finPlayerScale.x = -1;
        playerTf.localScale = finPlayerScale;

        //Make player fire the brain wave
        if (Input.GetButtonDown("Fire1")){
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            Vector2 dir = mousePos - new Vector2(fireOffset.position.x, fireOffset.position.y);

            //instantiate bullet
            GameObject bullet = Instantiate(Projectile, fireOffset.position, fireOffset.rotation);
            dir.Normalize();
            //let it go
            bullet.GetComponent<Rigidbody2D>().velocity = dir * fireSpeed;
        }
    }

    void OnCollisionStay2D(Collision2D collisionObject){
        Debug.Log(collisionObject.gameObject.name);
        //Check if the player is on the ground to jump
        if(playerTf.position.y - playerBc.size.y / 2 >= collisionObject.gameObject.transform.position.y + collisionObject.gameObject.GetComponent<BoxCollider2D>().size.y / 2){
            grounded = true;
        }
    }
}
