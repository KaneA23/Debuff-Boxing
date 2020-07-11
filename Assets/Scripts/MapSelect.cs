using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
    Author:     Chris Lamb
    Last edit:  07/04/2019
    Purpose:    Logic for the map selection.
*/

public class MapSelect : MonoBehaviour {

    // Functions to change between the Scenes using the Build Index, these will be linked with the button on the Map Select Menu

	public void PlayMap1()
    {
        SceneManager.LoadScene(sceneBuildIndex: 1);
    }

    public void PlayMap2()
    {
        SceneManager.LoadScene(sceneBuildIndex: 2);
    }

    public void PlayMap3()
    {
        SceneManager.LoadScene(sceneBuildIndex: 3);
    }

    public void PlayMap4()
    {
        SceneManager.LoadScene(sceneBuildIndex: 4);
    }
}
