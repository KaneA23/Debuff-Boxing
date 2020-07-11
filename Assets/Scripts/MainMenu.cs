using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    Author:     Chris Lamb
    Last edit:  17/02/2019
    Purpose:    Logic for the main menu.
*/

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    public UnityEventQueueSystem button;

    public void PlayGame()
    {
        
    }

    // Exits game
    public void QuitGame ()
    {
        Application.Quit();
    }
}
