using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public GameObject gameOverCanvas;

    void Update(){
        GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
        if(!player.GetComponent<PlayerAction>().alive){
            player.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0f, 0f);
            gameOverCanvas.SetActive(true);
            Time.timeScale = 0;
        }

        if(Input.GetButton("Restart")){
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if(player.GetComponent<PlayerAction>().success){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

		if(Input.GetKey("escape")) Application.Quit();
    }
}
