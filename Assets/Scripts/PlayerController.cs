using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    Author:     Chris Banton
    Last edit:  05/05/2019
    Purpose:    Controller for the player, contains code that allows them to move and jump. 
                Some varaibles in the file are used by the FistController file.
*/

public class PlayerController : MonoBehaviour
{
    // Index that tells us what player the user is
    [SerializeField]
    private int playerIndex;

    private float timer;

    // Tells us what direction the player is facing
    [SerializeField]
    private int playerDirection;

    // Tells us how much health the player has
    [SerializeField]
    private float playerHealth;

    // Tells us whether the player has been hit by the other player
    private bool beenPunched;

    // Tells us if the player is currently blocking          
    private bool isPlayerBlocking;

    // Tells us if the player is in the air
    private bool isPlayerJumping;

    // Tells us if the player has jumped off the wall
    private bool hasPlayerWallJumped;

    // Tells us what controller the player is using
    [SerializeField]
    private string controllerPrefix;

    // Get a reference to the player's fist
    [SerializeField]
    private GameObject fistObject;
    [SerializeField]
    private FistController fistScript;

    // Allows us to manipulate the players physics
    [SerializeField]
    public Rigidbody2D myRigidBody2D;

    // Get a refernce to the player's healthbar
    [SerializeField]
    private GameObject healthBar;
    [SerializeField]
    private GameObject healthText;

    private bool isWhosWho;

    // Define movement controls
    enum CONTROLS
    {
        WALK,
        JUMP,
        BLOCK,
    }

    // Use this for initialization
    void Start()
    {
        // Changes the players different colours.
        if (playerIndex == 1) // Player 1 = Cyan
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
            fistObject.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
        else if (playerIndex == 2) // Player 2 = Yellow
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            fistObject.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else if (playerIndex == 3) // Player 3 = Red
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            fistObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if (playerIndex == 4) // Player 4 = Green
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            fistObject.GetComponent<SpriteRenderer>().color = Color.green;
        }

        myRigidBody2D = GetComponent<Rigidbody2D>();
        fistScript = fistObject.GetComponent<FistController>();

        playerHealth = 100.0f; // Default health is 100
        beenPunched = false;
        isWhosWho = false;

        // If the player wasn't selected in the main menu
        if (PlayerCount.NumOfPlayers < playerIndex)
        {
            // Disable it
            healthText.SetActive(false);
            healthBar.SetActive(false);
            gameObject.SetActive(false);
        }

        // Sets player to a controller (4 players max)
        if (playerIndex == 1)
        {
            controllerPrefix = "P1";
        }
        else if (playerIndex == 2)
        {
            controllerPrefix = "P2";
        }
        else if (playerIndex == 3)
        {
            controllerPrefix = "P3";
        }
        else if (playerIndex == 4)
        {
            controllerPrefix = "P4";
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Destory the player if the have 0 health
        if (playerHealth <= 0)
        {
            gameObject.SetActive(false);
        }

        // Flip the players sprite to face the direction they are looking
        if (playerDirection == 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }

        // Set the players fist to be facing the same die=rection the player is
        fistScript.punchAnim.SetInteger("Direction", playerDirection);

        // Update the healthbar with the players health
        healthBar.GetComponent<Slider>().value = playerHealth;

        // Create a vector to store the players velocity
        Vector2 playerVelocity = myRigidBody2D.velocity;

        // Player movement
        // If the player hasn't been hit
        if (!beenPunched)
        {
            // And the player isn't blocking
            if (!isPlayerBlocking)
            {
                if (GetPlayerKey(CONTROLS.WALK) > 0)
                {
                    // Move right
                    playerVelocity += Vector2.right;
                    playerDirection = 1;
                }
                else if (GetPlayerKey(CONTROLS.WALK) < 0)
                {
                    // Move left
                    playerVelocity += Vector2.left;
                    playerDirection = 0;
                }
                if (GetPlayerKey(CONTROLS.JUMP) > 0 && !isPlayerJumping)
                {
                    // Jump
                    playerVelocity += (Vector2.up * 8.0f);

                    // Set flag
                    isPlayerJumping = true;
                }
            }

            // And the player is blocking
            if (GetPlayerKey(CONTROLS.BLOCK) > 0 && !isPlayerJumping)
            {
                // Set bool that reduces damage taken
                isPlayerBlocking = true;
                // Increase mass so the player can't be knocked back
                myRigidBody2D.mass = 1000;
                // Stop player movement
                playerVelocity.x = 0;

            }
            // When blocking ends
            else
            {
                // Reset bool and mass
                isPlayerBlocking = false;
                myRigidBody2D.mass = 1;
            }
        }

        // Clamp player velocity
        if (!beenPunched)
        {
            playerVelocity.x = Mathf.Clamp(playerVelocity.x, -10, 10);
            playerVelocity.y = Mathf.Clamp(playerVelocity.y, -20, 20);
        }

        // Set velocity
        myRigidBody2D.velocity = playerVelocity;

        // If Who's-Who buff is active, all players turn default (white).
        if (isWhosWho)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            fistObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else // Otherwise, they turn to their persojnal colour.
        {
            if (playerIndex == 1)
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
                fistObject.GetComponent<SpriteRenderer>().color = Color.cyan;
            }
            else if (playerIndex == 2)
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                fistObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
            else if (playerIndex == 3)
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                fistObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else if (playerIndex == 4)
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                fistObject.GetComponent<SpriteRenderer>().color = Color.green;
            }
        }
    }

    // Set up movement controls for the players
    float GetPlayerKey(CONTROLS requestedControls)
    {
        switch (requestedControls)
        {
            case CONTROLS.WALK:
                {
                    return Input.GetAxis(controllerPrefix + "Horizontal");
                }
            case CONTROLS.JUMP:
                {
                    return Input.GetAxis(controllerPrefix + "Jump");
                }
            case CONTROLS.BLOCK:
                {
                    return Input.GetAxis(controllerPrefix + "Block");
                }
        }
        return 0.0f;
    }

    // Resets jump (allow player to be able to jump again once touches floor or wall)
    void OnCollisionEnter2D(Collision2D collision)
    {
        // If the player touches the floor, they are no longer jumping
        if (collision.gameObject.tag == "Floor")
        {
            isPlayerJumping = false;
            hasPlayerWallJumped = false;
        }
        // The player can jump off a wall
        else if (collision.gameObject.tag == "Wall")
        {
            // Provided they have not already done so
            if (!hasPlayerWallJumped)
            {
                isPlayerJumping = false;
                hasPlayerWallJumped = true;
            }
        }
    }

    // Getters and setters for all variables
    public int GetPlayerIndex()
    {
        return playerIndex;
    }

    public int GetPlayerDirection()
    {
        return playerDirection;
    }

    public float GetPlayerHealth()
    {
        return playerHealth;
    }

    public void SetPlayerHealth(int newHealth)
    {
        playerHealth = newHealth;
    }

    public void TakeDamage(float damage)
    {
        playerHealth -= damage;
    }

    public bool IsPlayerPunched()
    {
        return beenPunched;
    }

    public bool IsPlayerBlocking()
    {
        return isPlayerBlocking;
    }

    public void SetPlayerPunched(bool isPunched)
    {
        beenPunched = isPunched;
    }

    public bool PlayerJumping()
    {
        return isPlayerJumping;
    }

    public string GetControllerPrefix()
    {
        return controllerPrefix;
    }

    // Code added by Kane

    // Puts players' health to 1.
    // (1 hit will defeat player).
    public void InstaDeath()
    {
        if (playerHealth > 1.0f)
        {
            playerHealth -= 1.0f;
        }
    }

    // All players get full health (100).
    public void MaxHealth()
    {
        playerHealth = 100.0f;
    }

    // Changes player's health randomly, changes differently for each player.
    // To minimise the disability, health will be between 20 and 80.
    public void RandHealth()
    {
        playerHealth = Random.Range(20, 80);
    }

    // Puts all players' healths to 50 (half).
    public void SplitHealth()
    {
        playerHealth = 50.0f;
    }

    // Decreases all player's health overtime.
    // Decrease about 0.025 each frame.
    public void PoisonMist()
    {
        if (playerHealth > 15.0f)
        {
            playerHealth -= 0.025f;
        }
    }

    public void WhosWho()
    {
        isWhosWho = true;
    }

    public void RemoveWhosWho()
    {
        isWhosWho = false;
    }

    // Code added by Kane stops
}
