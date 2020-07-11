using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    Author:     Chris Banton
    Last edit:  29/04/2019
    Purpose:    Checks how many players are needed, depending on number of players selected
*/

public class PlayerCount : MonoBehaviour
{
    //private static int numOfPlayers;
    public static int NumOfPlayers { get; private set; }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        NumOfPlayers = gameObject.GetComponent<Dropdown>().value + 2;
    }

    public static int GetNumOfPlayers()
    {
        return NumOfPlayers;
    }
}
