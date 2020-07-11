using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    Author:     Kane Adams
    Last edit:  05/05/2019
    Purpose:    Controls what buffs are chosen for game.
                Contains code to randomly select the buff.
*/

public class BuffScripts : MonoBehaviour
{
    // Get a reference to the players
    public GameObject[] Players;

    public GameObject[] Fists;

    // GameState determines what buff is applied (for 30 seconds)
    private int GameState;

    // Booleans to determine which buff is active.
    private bool isPoisonBuff = false;
    private bool isMegaPunchBuff = false;
    private bool isBabyPunchBuff = false;
    private bool isWhoWhoBuff = false;

    // Boolan to determine whether the player has activated a particular buff for the match.
    private bool buffIsValid;

    // Timer before next buff.
    private float Timer = 30.0f;

    // Text that shows what the next buff will be (in 10 seconds).
    [SerializeField]
    Text NextBuffText;

    // Text that shows the the current buff is.
    [SerializeField]
    Text ActiveBuffText;

    public AudioClip buffClip;

    public AudioSource buffSource;

    // Use this for initialization
    void Start()
    {
        // Finds the required GameObjects required for 'BuffScript.cs' and assigns them to variables.
        Players = GameObject.FindGameObjectsWithTag("Player");
        NextBuffText = GameObject.Find("NextBuffText").GetComponent<Text>();
        ActiveBuffText = GameObject.Find("ActiveBuffText").GetComponent<Text>();
        Fists = GameObject.FindGameObjectsWithTag("Fist");

        buffSource.clip = buffClip;

        ChangeTimerText("Buff Timer: " + (int)Timer);

        ChooseNextBuff();
    }

    // Update is called once per frame
    void Update()
    {
        // Working timer that resets after 30 seconds.
        Timer -= Time.deltaTime;
        ChangeTimerText("Buff Timer: " + (int)Timer);

        // Activates specific buffs every frame (e.g. if changes throughout the buff timer).

        // Activates Poison Air buff.
        if (isPoisonBuff)
        {
            StartCoroutine(PoisonTimer());
        }

        // Activates Damage Up buff.
        if (isMegaPunchBuff)
        {
            for (int i = 0; i < Fists.Length; i++)
            {
                Fists[i].GetComponent<FistController>().MegaPunch();
            }
        }

        // Activates Damage Down buff.
        if (isBabyPunchBuff)
        {
            for (int i = 0; i < Fists.Length; i++)
            {
                Fists[i].GetComponent<FistController>().BabyPunch();
            }
        }

        // Once 10 seconds are left, next buff is shown at sneak peak.
        if ((int)Timer == 10)
        {
            // Code edited by Kane as of 5/5/2019

            // Remove current buff, before adding new buff.

            // Removes Poison De-buff.
            isPoisonBuff = false;

            // Remove buffs related to damage.
            if (isMegaPunchBuff || isBabyPunchBuff)
            {
                for (int i = 0; i < Fists.Length; i++)
                {
                    Fists[i].GetComponent<FistController>().RemovePunchMultiplier();
                }
            }

            // Removes Who's-Who buff
            if (isWhoWhoBuff)
            {
                for (int i = 0; i < Players.Length; i++)
                {
                    Players[i].GetComponent<PlayerController>().RemoveWhosWho();
                }
            }

            ChangeNextBuffText("Next Buff: " + NextBuff(GameState));
        }

        // Once timer reaches zero, buff is applied, timer resets (to 30 seconds), and next buff is chosen.
        if ((int)Timer < 0)
        {
            // Plays buff alarm.
            buffSource.Play();

            ChangeActiveBuffText(NextBuff(GameState));

            // Changes Buff.
            AddBuff(GameState);

            // Resets timer (for next buff).
            Timer = 30;

            // Calls random buff selector.
            ChooseNextBuff();

            // Resets Next buff text to default (nothing).
            ChangeNextBuffText("");

            StartCoroutine(ActiveBuffTextTimer());
        }
    }

    // Code edited by Kane as of 1/4/2019
    // Changes the timer on the U.I. timer.
    private void ChangeTimerText(string text)
    {
        GetComponent<Text>().text = text;
    }

    // Randomly selects next buff (depoending whether it is active).
    public void ChooseNextBuff()
    {
        // Coded edited by Chris banton as of 04/05/2019

        // Checks whether the Buff is available (player hasn't deselected it).
        do
        {
            GameState = Random.Range(0, 8);
            buffIsValid = true;

            // Checks whether the randomly selected buff is available until one is.
            for (int i = 0; i < BuffSelector.removedBuffs.Length; i++)
            {
                if (GameState == BuffSelector.removedBuffs[i])
                {
                    buffIsValid = false;
                }
            }
        } while (!buffIsValid);
    }

    // Changes the text for the next buff sneak peak U.I.
    private void ChangeNextBuffText(string text)
    {
        NextBuffText.text = text;
    }

    // Changes the text for what buff has just activated.
    private void ChangeActiveBuffText(string text)
    {
        ActiveBuffText.text = text;
    }

    // Code edited by Kane as of 6/5/2019
    private void AddBuff(int GameState)
    {
        // Code edited by Chris Banton as of 27/02/2019
        switch (GameState)
        {
            case 0:
                {
                    // 0 = Sudden death (1 Health)
                    for (int i = 0; i < Players.Length; i++)
                    {
                        Players[i].GetComponent<PlayerController>().InstaDeath();   
                    }
                    GameState = 10;
                    break;
                }
            case 1:
                {
                    // 1 = Max Health (100)
                    for (int i = 0; i < Players.Length; i++)
                    {
                        Players[i].GetComponent<PlayerController>().MaxHealth();  
                    }
                    GameState = 10;
                    break;
                }
            case 2:
                {
                    // 2 = Random Health (between 20 and 80)
                    for (int i = 0; i < Players.Length; i++)
                    {
                        Players[i].GetComponent<PlayerController>().RandHealth();  
                    }
                    GameState = 10;
                    break;
                }
            case 3:
                {
                    // 3 = Split Health (50)
                    for (int i = 0; i < Players.Length; i++)
                    {
                        Players[i].GetComponent<PlayerController>().SplitHealth();
                    }
                    GameState = 10;
                    break;
                }
            case 4:
                {
                    // 4 = Poison Mist
                    for (int i = 0; i < Players.Length; i++)
                    {
                        isPoisonBuff = true;
                    }
                    GameState = 10;
                    break;
                }
            case 5:
                {
                    // 5 = Damage Up
                    for (int i = 0; i < Players.Length; i++)
                    {
                        isMegaPunchBuff = true;
                    }
                    GameState = 10;
                    break;
                }
            case 6:
                {
                    // 6 = Damage Down
                    for (int i = 0; i < Players.Length; i++)
                    {
                        isBabyPunchBuff = true;
                        GameState = 10;
                    }
                    break;
                }
            case 7:
                {
                    // 7 = Who's-Who
                    for (int i = 0; i < Players.Length; i++)
                    {
                        Players[i].GetComponent<PlayerController>().WhosWho();
                    }
                    isWhoWhoBuff = true;
                    GameState = 10;
                    break;
                }
        }
    }

    // Code edited by Kane on 1/4/2019
    private string NextBuff(int GameState)
    {
        switch (GameState)
        {
            case 0:
                {
                    return "Sudden Death";
                }
            case 1:
                {
                    return "Max Health";
                }
            case 2:
                {
                    return "Random Health";
                }
            case 3:
                {
                    return "Split Health";
                }
            case 4:
                {
                    return "Poison Mist";
                }
            case 5:
                {
                    return "Damage Up";
                }
            case 6:
                {
                    return "Damage Down";
                }
            case 7:
                {
                    return "Who's-Who?";
                }
        }
        return "";
    }

    // Creates a 2 second delay before Poison mist is activated.
    IEnumerator PoisonTimer()
    {
        yield return new WaitForSeconds(2);

        for (int i = 0; i < Players.Length; i++)
        {
            Players[i].GetComponent<PlayerController>().PoisonMist();
        }
    }

    // Keeps the Current buff text up for 2 seconds.
    IEnumerator ActiveBuffTextTimer()
    {
        yield return new WaitForSeconds(2);

        ChangeActiveBuffText("");
    }
}
