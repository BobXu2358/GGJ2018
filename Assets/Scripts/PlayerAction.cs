using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAction : MonoBehaviour {
    public Transform playerTf;
    public BoxCollider2D playerBc;
    public Rigidbody2D playerRb;
    public Transform mapGroundTf;
    public BoxCollider2D mapGroundBc;
    public float moveSpeed;
    public float jumpForce;
    bool grounded = true;

    void FixedUpdate(){
        Vector2 playerSpeed = playerRb.velocity;
        playerSpeed.x = Input.GetAxis("Horizontal") * moveSpeed;
        playerRb.velocity = playerSpeed;
        if(grounded && Input.GetButton("Jump")){
            grounded = false;
            playerRb.AddForce(new Vector2(0, jumpForce));
        }
    }

    void OnCollisionStay2D(Collision2D collisionObject){
        if(playerTf.position.y - playerBc.size.y / 2 >= collisionObject.gameObject.transform.position.y + collisionObject.gameObject.GetComponent<BoxCollider2D>().size.y / 2){
            grounded = true;
        }
    }
}
