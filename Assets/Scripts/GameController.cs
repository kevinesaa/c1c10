using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject player;

    public UIController uiController;

    // Use this for initialization
    void Start () 
    {
       
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public void play()
    {
        player.SetActive(true);
        //player.GetComponent<PlayerController>().RestarPLayer();
        RestartGame();
        uiController.play();

    }

    public void playFromPause()
    {
        StartCoroutine(playFromPauseCoroutine());
    }

    public void pause()
    {
        uiController.pause();
    }

    public void GameOver()
    {

    }

    public void ToMainMenu()
    {
        RestartGame();
        player.SetActive(false);
        uiController.ToMainMenu();
    }

    public void RestartGame()
    {
        //player.transform.position = sTartPoint;
        //player.GetComponent<PlayerController>().RestarPLayer();
    }

    private IEnumerator playFromPauseCoroutine()
    {
        uiController.playFromPause();
        yield return new WaitForSeconds(1f);
    }

}
