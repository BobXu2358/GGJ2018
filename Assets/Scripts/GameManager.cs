using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    int restartTime = 0;

    void Update(){
        if(Input.GetButton("Restart"))
            restartTime++;
        else
            restartTime = 0;

        GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
        if((!player.GetComponent<PlayerAction>().alive) || (restartTime >= 45)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if(player.GetComponent<PlayerAction>().success){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
