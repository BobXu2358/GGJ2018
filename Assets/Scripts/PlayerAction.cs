using UnityEngine;

public class PlayerAction : MonoBehaviour {
    public Transform playerTf;
    public Rigidbody playerRb;
    public float moveSpeed;
    public float jumpForce;
    bool grounded = true;

    void FixedUpdate(){
        Vector3 moveOffset = new Vector3(0, 0, 0);
        moveOffset.x += Input.GetAxisRaw("Horizontal") * moveSpeed;
        playerTf.position += moveOffset;

        if(Input.GetButton("Jump") && grounded){
            grounded = false;
            playerRb.AddForce(0, jumpForce, 0);
        }
    }

    void OnCollisionEnter(Collision collisionObject){
        if(collisionObject.gameObject != gameObject){
            grounded = true;
        }
    }
}
