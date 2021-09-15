using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    //public GameObject startPanel;
    public GameObject gamePanel;
    // GameObject pausePanel;
    //public GameObject gameOverPanel;

    private Animator pauseAnim;
    private Image fundido;

	// Use this for initialization
	/*void Start () 
    {
        //fundido = gameOverPanel.GetComponent<Image>();
        //pauseAnim = pausePanel.GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}*/

    public void play()
    {
        playPanels();
    }

    public void playFromPause()
    {
        StartCoroutine(playFromPauseCoroutine());
    }

    public void playPanels()
    {
        gamePanel.SetActive(true);
        //startPanel.SetActive(false);
        //pausePanel.SetActive(false);
        //gameOverPanel.SetActive(false);
        fundido.CrossFadeAlpha(0, 1, false);

    }

    public void pause()
    {
        //pausePanel.SetActive(true);
        gamePanel.SetActive(false);
        pauseAnim.SetTrigger("pause_in");

    }

    public void ToMainMenu()
    {
        gamePanel.SetActive(false);
        //pausePanel.SetActive(false);
        //gameOverPanel.SetActive(false);
        //startPanel.SetActive(true);
    }

    public void gameOver()
    {
        fundido.CrossFadeAlpha(1, 1, false);
        //gameOverPanel.SetActive(true);
        gamePanel.SetActive(false);
    }

    IEnumerator playFromPauseCoroutine()
    {
        pauseAnim.SetTrigger("pause_out");
        yield return new WaitForSeconds(1f);
        playPanels();
    }
}
