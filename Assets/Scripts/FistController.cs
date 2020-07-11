using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Author:     Chris Banton
    Last edit:  05/05/2019
    Purpose:    Controller for the players fist, contains the code that launches players back when they are hit. 
                Also contains code to set triggers in the punchAnimation animator.
*/

public class FistController : MonoBehaviour
{
    // Get a refrence to the player
    public GameObject playerObject;
    public PlayerController playerScript;

    public bool isMegaPunchBuff = false;
    public bool isBabyPunchBuff = false;

    // Get the position of the fist
    float fistPosX;

    // Set the force of the punch
    float punchForce;

    // Set the damage of the punch
    int punchDamage;

    // Set how long a player is stunned after being attacked
    int stunDuration;

    // Stores how long an animation takes
    public float punchDuration;

    // Set whether the player is punching
    bool isPunching;

    // Stores what attack animation is being played
    string attackAnimation;

    // Vector thats used to tell the player what direction they should move, after they've been hit
    Vector2 directionToMove;

    // Stores the punch animations of the player
    public Animator punchAnim;

    // Array that stores all possible attack animations
    public AnimationClip[] animations;

    public AudioClip punchClip;

    public AudioSource punchSource;

    // Use this for initialization
    void Start()
    {
        // Collects the gameObject related to the attack animations
        punchAnim = gameObject.GetComponent<Animator>();
        playerScript = playerObject.GetComponent<PlayerController>();
        fistPosX = gameObject.GetComponent<Transform>().position.x;

        // Stores the values related to the attack animations
        punchForce = 8.0f;
        punchDamage = 10; // Default attack strength, changes depending on attack used and buffs
        stunDuration = 1;

        animations = punchAnim.runtimeAnimatorController.animationClips;

        punchSource.clip = punchClip;
    }

    // Update is called once per frame
    void Update()
    {
        // If the player changes direction, the fist changes sides
        if (playerScript.GetPlayerDirection() == 0 && fistPosX > 0)
        {
            fistPosX = -1;
        }
        if (playerScript.GetPlayerDirection() == 1 && fistPosX < 0)
        {
            fistPosX *= -1;
            gameObject.transform.position.Set(fistPosX * -1, 0, 0);
        }

        //Animation code added by Kane

        //As of 27/01/2019 animation code was altered by Chris Banton

        // Can only attack if the player is not blocking
        if (!playerScript.IsPlayerBlocking())
        {
            // Code edited by Kane as of 2/5/2019

            // For punching
            if (Input.GetButtonDown(playerScript.GetControllerPrefix() + "Punch"))
            {
                isPunching = true;

                // Light Attack strength.
                if (isMegaPunchBuff)
                {
                    punchDamage = 20;
                }
                else if (isBabyPunchBuff)
                {
                    punchDamage = 5;
                }
                else
                {
                    punchDamage = 10;
                }

                // Decides which Punch animation to play, depending on which direction the player is facing
                switch (playerScript.GetPlayerDirection())
                {
                    case 1: // Right
                        {
                            if (!PlayerIsAttacking())
                            {
                                punchAnim.SetTrigger("doRightPunch");
                                attackAnimation = "RightPunch";
                                SetPunchDuration();
                            }
                            break;
                        }
                    case 0: // Left
                        {
                            if (!PlayerIsAttacking())
                            {
                                punchAnim.SetTrigger("doLeftPunch");
                                attackAnimation = "LeftPunch";
                                SetPunchDuration();
                            }
                            break;
                        }
                }
            }

            // For uppercutting
            else if (Input.GetButtonDown(playerScript.GetControllerPrefix() + "Uppercut"))
            {
                isPunching = true;

                // Heavy attack damage.
                if (isMegaPunchBuff)
                {
                    punchDamage = 30;
                }
                else if (isBabyPunchBuff)
                {
                    punchDamage = 8;
                }
                else
                {
                    punchDamage = 15;
                }

                // Decides which Uppercut animation to play, depending on which direction the player is facing
                switch (playerScript.GetPlayerDirection())
                {
                    case 1: // Right
                        {
                            if (!PlayerIsAttacking())
                            {
                                punchAnim.SetTrigger("doRightUppercut");
                                attackAnimation = "RightUppercut";
                                SetPunchDuration();
                            }
                            break;
                        }
                    case 0: // Left
                        {
                            if (!PlayerIsAttacking())
                            {
                                punchAnim.SetTrigger("doLeftUppercut");
                                attackAnimation = "LeftUppercut";
                                SetPunchDuration();
                            }
                            break;
                        }
                }
            }
            // Code added by Kane stops
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // If you punch another player
        if (collision.gameObject.tag == "Player" && isPunching)
        {
            // They get hit
            PlayerController otherPlayer = collision.gameObject.GetComponent<PlayerController>();

            // Play sound effect
            punchSource.Play();

            // They take damage
            if (!otherPlayer.IsPlayerPunched())
            {
                // Attacked player takes damage
                if (otherPlayer.IsPlayerBlocking())
                {
                    otherPlayer.TakeDamage(punchDamage / 100);
                }
                else
                {
                    otherPlayer.TakeDamage(punchDamage);
                }
            }

            // Set beenPunched to true
            otherPlayer.SetPlayerPunched(true);

            // Start the timer to reset beenPunched
            StartCoroutine(StunTimer(otherPlayer));

            StartCoroutine(PunchTime());

            // Stop the player
            otherPlayer.myRigidBody2D.velocity = Vector2.zero;

            // Then immediately move the player in the desired direction
            if (!otherPlayer.IsPlayerBlocking())
            {
                directionToMove = -(gameObject.transform.position - otherPlayer.transform.position).normalized;
                otherPlayer.myRigidBody2D.velocity += directionToMove * punchForce;
            }

        }
    }

    // The attack length of all of the attack animations are stored in punchDuration
    public void SetPunchDuration()
    {
        foreach (AnimationClip clip in animations)
        {
            if (clip.name == attackAnimation)
            {
                punchDuration = clip.length;
            }
        }
    }

    // Determines whether the player is attacking
    public bool PlayerIsAttacking()
    {
        if (punchAnim.GetCurrentAnimatorStateInfo(0).IsName("RightPunch") || punchAnim.GetCurrentAnimatorStateInfo(0).IsName("LeftPunch") ||
            punchAnim.GetCurrentAnimatorStateInfo(0).IsName("RightUppercut") || punchAnim.GetCurrentAnimatorStateInfo(0).IsName("LeftUppercut"))
        {
            return true;
        }
        return false;
    }

    // Stops the player from being able to attack if hit
    IEnumerator StunTimer(PlayerController otherPlayer)
    {
        yield return new WaitForSeconds(stunDuration);
        otherPlayer.SetPlayerPunched(false);
    }

    // Stops the player from being able to do a second attack at the same time as the current attack
    IEnumerator PunchTime()
    {
        yield return new WaitForSeconds(punchDuration);
        isPunching = false;
    }

    // Doubles punchDamage.
    public void MegaPunch()
    {
        isMegaPunchBuff = true;
    }

    // Halves all attack strengths.
    public void BabyPunch()
    {
        isBabyPunchBuff = true;
    }

    // Returns punchDamage to default strength.
    public void RemovePunchMultiplier()
    {
        isMegaPunchBuff = false;
        isBabyPunchBuff = false;
    }
}
