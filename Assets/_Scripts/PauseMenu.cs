using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

    public GameObject pauseUI;

    public GameObject mobileInput;
    private bool paused = false;



    public void Resume()
    {
        this.pauseUI.SetActive(false);
        //Time.timeScale = 1;
    }

    public void Restart()
    {
        // reload the same level
        Application.LoadLevel(Application.loadedLevel);
    }

    public void MainMenu()
    {
        // Load another level
        // Application.LoadLevel("Menu");
        Application.LoadLevel(Application.loadedLevel);
    }

    public void Exit()
    {
        Application.Quit();
    }

	// Use this for initialization
	void Start () {
        this.pauseUI.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Pause"))
        {
            this.paused = !paused;
        }
        if (paused)
        {
            this.pauseUI.SetActive(true);
            this.mobileInput.SetActive(false);
            Time.timeScale = 0;
        }
        if (!paused)
        {
            this.mobileInput.SetActive(true);
            this.pauseUI.SetActive(false);
            Time.timeScale = 1;
        }      
	}
}
