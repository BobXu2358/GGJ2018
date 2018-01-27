using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public GameObject gameOverCanvas;

    void Update(){
        GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
        if(!player.GetComponent<PlayerAction>().alive){
            GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerAction>().enabled = false;
            gameOverCanvas.SetActive(true);
        }

        if(Input.GetButton("Restart")){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if(player.GetComponent<PlayerAction>().success){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

		if(Input.GetKey("escape")) Application.Quit();
    }
}
