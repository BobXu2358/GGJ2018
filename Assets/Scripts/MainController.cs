using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour {
    public Transform playerTf;
    public BoxCollider2D playerBc;
    public Transform mapGroundTf;
    public BoxCollider2D mapGroundBc;

    void FixedUpdate(){
        Vector3 finPlayerPos = playerTf.position;
        if(finPlayerPos.x - playerBc.size.x / 2 <= mapGroundTf.position.x - mapGroundBc.size.x / 2)
            finPlayerPos.x = mapGroundTf.position.x - mapGroundBc.size.x / 2 + playerBc.size.x / 2;
        if(finPlayerPos.x + playerBc.size.x / 2 >= mapGroundTf.position.x + mapGroundBc.size.x / 2)
            finPlayerPos.x = mapGroundTf.position.x + mapGroundBc.size.x / 2 - playerBc.size.x / 2;
        playerTf.position = finPlayerPos;
        if(playerTf.position.y - playerBc.size.y / 2 <= mapGroundTf.position.y + mapGroundBc.size.y / 2)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Vector3 finPlayerScale = playerTf.localScale;
        if(Input.GetAxisRaw("Horizontal") == 1) finPlayerScale.x = 1;
        if(Input.GetAxisRaw("Horizontal") == -1) finPlayerScale.x = -1;
        playerTf.localScale = finPlayerScale;
    }
}
