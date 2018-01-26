using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAction : MonoBehaviour {
    public Transform playerTf;
    public Rigidbody playerRb;
    public Transform mapGroundTf;
    public float moveSpeed;
    public float jumpForce;
    bool grounded = true;

    void FixedUpdate(){
        Vector3 moveOffset = new Vector3(0, 0, 0);
        moveOffset.x += Input.GetAxisRaw("Horizontal") * moveSpeed;
        Vector3 finPlayerPos = playerTf.position + moveOffset;

        if(finPlayerPos.x - playerTf.lossyScale.x / 2 <= mapGroundTf.position.x - mapGroundTf.lossyScale.x / 2)
            finPlayerPos.x = mapGroundTf.position.x - mapGroundTf.lossyScale.x / 2 + playerTf.lossyScale.x / 2;
        if(finPlayerPos.x + playerTf.lossyScale.x / 2 >= mapGroundTf.position.x + mapGroundTf.lossyScale.x / 2)
            finPlayerPos.x = mapGroundTf.position.x + mapGroundTf.lossyScale.x / 2 - playerTf.lossyScale.x / 2;

        playerTf.position = finPlayerPos;

        if(playerTf.position.y - playerTf.lossyScale.y / 2 <= mapGroundTf.position.y + mapGroundTf.lossyScale.y / 2)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if(grounded && Input.GetButton("Jump")){
            grounded = false;
            playerRb.AddForce(0, jumpForce, 0);
        }

        if(Input.GetButtonDown("Fire1")){

        }
    }

    void OnCollisionStay(Collision collisionObject){
        if(playerTf.position.y - playerTf.lossyScale.y / 2 >= collisionObject.gameObject.transform.position.y + collisionObject.gameObject.transform.lossyScale.y / 2){
            grounded = true;
        }
    }
}
