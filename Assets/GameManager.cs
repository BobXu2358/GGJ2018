using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    void Update(){
        GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
        if(!player.GetComponent<PlayerAction>().alive){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
