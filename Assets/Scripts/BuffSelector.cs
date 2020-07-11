using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    Author:     Chris Banton
    Last edit:  04/05/2019
    Purpose:    Adds all disabled buffs to a static array that wil be accessed by buff script class.
*/

public class BuffSelector : MonoBehaviour
{
    // Array that contains all buffs in the game
    Toggle[] allBuffs;

    // Array that contains all buffs to be removed from play
    public static int[] removedBuffs = new int[1];

    // Use this for initialization
    void Start()
    {
        // Add all buff to array
        allBuffs = GetComponentsInChildren<Toggle>();
    }

    // Update is called once per frame
    void Update()
    {
        // Temporary array to store ints
        int[] temp;

        // If a buff was unchecked, add it to the removedBuffs array
        for (int i = 0; i < allBuffs.Length; i++)
        {
            if (!allBuffs[i].isOn)
            {
                temp = new int[removedBuffs.Length + 1];
                for (int j = 0; j < removedBuffs.Length; j++)
                {
                    temp[j] = removedBuffs[j];
                }
                temp[temp.Length - 1] = i;
                removedBuffs = temp;
            }
        }
    }
}
