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
        Vector3 moveOffset = new Vector3(0, 0, 0);
        moveOffset.x += Input.GetAxis("Horizontal") * moveSpeed;
        Vector3 finPlayerPos = playerTf.position + moveOffset;

        if(finPlayerPos.x - playerBc.size.x / 2 <= mapGroundTf.position.x - mapGroundBc.size.x / 2)
            finPlayerPos.x = mapGroundTf.position.x - mapGroundBc.size.x / 2 + playerBc.size.x / 2;
        if(finPlayerPos.x + playerBc.size.x / 2 >= mapGroundTf.position.x + mapGroundBc.size.x / 2)
            finPlayerPos.x = mapGroundTf.position.x + mapGroundBc.size.x / 2 - playerBc.size.x / 2;

        playerTf.position = finPlayerPos;

        if(playerTf.position.y - playerBc.size.y / 2 <= mapGroundTf.position.y + mapGroundBc.size.y / 2)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

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
