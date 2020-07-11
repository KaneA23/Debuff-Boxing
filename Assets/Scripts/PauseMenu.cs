using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    Author:     Chris Lamb
    Last edit:  17/02/2019
    Purpose:    Logic for the pause menu.
*/

public class PauseMenu : MonoBehaviour {

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetAxis("Pause") > 0)
        { 
            QuitGame();
        }
        if (!GameIsPaused)
        {
            Resume();
        }
	}

    public void Resume ()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause ()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    
    // Exits game
    public void QuitGame()
    {
        Application.Quit();
    }

}
