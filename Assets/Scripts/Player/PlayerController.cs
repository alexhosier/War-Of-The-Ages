using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    
    #region Variables

    [SerializeField] [Range(0, 100)] private float health;
    
    [Header("Player Movement Options")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private string floorTag;
    
    [Header("Player Attack Options")]
    [SerializeField] private float attackDelay;
    [SerializeField] private float attackRange;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private PlayerInputs playerInputs;
    private ShopController shopController;
    private bool isGrounded;
    private float nextAttack;
    private PlayerUI playerUI;
    private int playerScore;
    private AudioSource audioSource;
    
    #endregion

    #region Start method

    private void Start() {
        // Init the save system
        SaveSystem.Init();
        
        // Fetch the rigidbody2d from the player
        rb = GetComponent<Rigidbody2D>();
        
        // Fetch the SpriteRender
        sr = GetComponent<SpriteRenderer>();

        // Fetch the playerUI
        playerUI = GetComponent<PlayerUI>();
        
        // Fetch the AudioSource
        audioSource = GetComponent<AudioSource>();
        
        // Fetch the shop controller
        shopController = GameObject.Find("Scripts").GetComponent<ShopController>();
        
        // Set the health to initial value
        health = 100;
        
        // Set the timescale
        Time.timeScale = 1f;
    }

    #endregion
    
    #region Player input system
    
    // Init player inputs
    private void Awake() {
        playerInputs = new PlayerInputs();
    }

    private void OnEnable() {
        playerInputs.Enable();
    }

    private void OnDisable() {
        playerInputs.Disable();
    }
    
    #endregion

    #region Update method

    private void Update() {
        // Fetch the movement left and right
        float moveX = playerInputs.Player.Horizontal.ReadValue<float>();

        // Move the player
        transform.Translate(Vector2.right * Time.deltaTime * moveSpeed * moveX);

        // Change the direction of the player sprite
        if (moveX < 0)
        {
            // Flip the player
            sr.flipX = true;
        }
        else
        {
            // Unflip the player
            sr.flipX = false;
        }

        // Check the health on the player
        if (health <= 0)
        {
            // Save the players score
            SaveSystem.Save(playerScore);
            
            // Delete the playerpref
            PlayerPrefs.DeleteKey("PlayerName");
            
            // Load the mennu
            SceneManager.LoadScene(0);
        }

        // If the player kills all of the enemies
        if (playerScore == 10)
        {
            // Start the end game procuedure
            StartCoroutine(EndGame());
        }
        
        #region Jump functionality

        // Jump functionality
        if(playerInputs.Player.Jump.triggered && isGrounded) {
            // Apply force
            rb.AddForce(Vector2.up * jumpForce);

            // Set false
            isGrounded = false;
        }

        #endregion
        
        #region Attack functionality

        // Attack functionality
        if(playerInputs.Player.Attack.triggered) {
            if(Time.time > nextAttack) {
                // Set next attack time
                nextAttack = Time.time + attackDelay;

                // Draw raycast
                RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.right * attackRange);
                
                // Play the sound
                audioSource.Play();
                
                // Check if it hit an enemy
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    // Set the AI's health to nothing
                    hit.collider.gameObject.GetComponent<AIController>().aiHealth = 0;
                    
                    // Increment player score
                    playerScore += 1;
                    
                    // Update score value
                    playerUI.UpdateScoreUI(playerScore);
                }
            }
        }

        #endregion
    }

    #endregion

    private IEnumerator EndGame()
    {
        // Save the players data
        SaveSystem.Save(playerScore);
        
        // Delete the players name
        PlayerPrefs.DeleteKey("PlayerName");

        // Wait for 5 seconds
        yield return new WaitForSeconds(5);
        
        // Load the leaderboard
        SceneManager.LoadScene(2);
    }

    #region Collision detection

    // If the player enters a collision
    private void OnCollisionEnter2D(Collision2D coll) {
        // If the collision was with the floor
        if(coll.gameObject.CompareTag(floorTag)) {
            // Set true
            isGrounded = true;
        }
    }

    // If the player stays in the trigger
    private void OnTriggerStay2D(Collider2D other)
    {
        // If the player is by the shop and presses the interaction button
        if (other.gameObject.name == "Shop" && playerInputs.Player.Use.triggered)
        {
            // Open the shop
            shopController.OpenShop();
        }
    }

    #endregion

    #region RemoveHealth method

    public void RemoveHealth(int toRemove)
    {
        // Remove the health
        health -= toRemove;
        
        // Convert float to int
        int healthToParse = (int) health;
        
        // Update the UI
        playerUI.UpdateHealthUI(healthToParse);
    }

    #endregion
    
}
