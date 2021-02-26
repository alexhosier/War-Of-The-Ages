using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class AIController : MonoBehaviour
{
    #region Variables

    private enum AIStates { Idle, Patrolling, Attacking, Dead }

    [Header("AI Health Information")] 
    [Range(0, 100)] public int aiHealth;
    
    [Header("AI State Information")]
    [SerializeField] private AIStates currentAIState;
    [SerializeField] private float idleDuration;
    [SerializeField] private float patrolDuration;
    [SerializeField] private float deathLifetime;
    [SerializeField] private float moveSpeed;

    [Header("AI Attack Information")] 
    [SerializeField] private float attackRange; 
    [SerializeField] private float attackDelay;

    [Header("AI GameObject References")] 
    [SerializeField] private Transform rayPoint;
    [SerializeField] private Transform playerPosition;

    private float idleTimer;
    private float patrolTimer;
    private float deadTimer;
    
    private float nextAttack;
    private bool isMovingRight;
    private SpriteRenderer sp;

    #endregion

    #region Start method

    private void Start()
    {
        // Set the initial state
        currentAIState = AIStates.Idle;

        // Set the initial health value
        aiHealth = 100;
        
        // Set the length of the AI states
        idleDuration = Random.Range(3, 8);
        patrolDuration = Random.Range(5, 10);
        
        // Set to moving right
        isMovingRight = true;
        
        // Fetch the sprite renderer
        sp = GetComponent<SpriteRenderer>();
    }

    #endregion

    #region Update method

    private void Update()
    {
        // Check if the AI has died
        if (aiHealth <= 0)
        {
            // Reset timer
            idleTimer = 0;
            
            // Change to the death state
            currentAIState = AIStates.Dead;
        }

        switch (currentAIState)
        {
            #region Idle state

            case AIStates.Idle:

                // Increment timer
                idleTimer += Time.deltaTime;

                // Check if the player is in range for an attack
                if (Vector2.Distance(transform.position, playerPosition.position) < attackRange)
                {
                    // Change to the attacking state
                    currentAIState = AIStates.Attacking;
                }
                
                // Check timer
                if (idleTimer >= idleDuration)
                {
                    // Change to the patrolling state
                    currentAIState = AIStates.Patrolling;
                    
                    // Reset timer
                    idleTimer = 0;
                }
                
                break;

            #endregion

            #region Patrolling state

            case AIStates.Patrolling:

                // Increment timer
                patrolTimer += Time.deltaTime;

                // Move player
                transform.Translate(Vector2.right * Time.deltaTime * moveSpeed);
                
                // Send a raycast down to the ground
                RaycastHit2D floorCheck = Physics2D.Raycast(rayPoint.position, Vector2.down);

                // Check if the floor is still beneath the player
                if (floorCheck.collider == null)
                {
                    if (isMovingRight)
                    {
                        // Set moving left
                        isMovingRight = false;

                        // Flip the AI
                        transform.eulerAngles = new Vector3(0, -180, 0);
                    }
                    else
                    {
                        // Set moving right
                        isMovingRight = true;
                        
                        // Flip the AI
                        transform.eulerAngles = new Vector3(0, 0, 0);
                    }
                }

                // Check if the player is in range for an attack
                if (Vector2.Distance(transform.position, playerPosition.position) < attackRange)
                {
                    // Change to the attacking state
                    currentAIState = AIStates.Attacking;
                }
                
                // Check timer 
                if (patrolTimer >= patrolDuration)
                {
                    // Change to the idle state
                    currentAIState = AIStates.Idle;
                    
                    // Reset timer
                    patrolTimer = 0;
                }
                
                break;  

            #endregion

            #region Attacking state

            case AIStates.Attacking:
                
                // Check if the player has escaped the enemy
                if (Vector2.Distance(transform.position, playerPosition.position) > attackRange)
                {
                    // Change to the patrolling state
                    currentAIState = AIStates.Patrolling;
                }
                
                // Attack the player
                if (Time.time > nextAttack)
                {
                    // Set the next time to attack
                    nextAttack = Time.time + attackDelay;

                    RaycastHit2D hit;
                    
                    // Shoot out a ray cast
                    if (transform.position.x < playerPosition.position.x)
                    {
                        // Shoot out a raycast to the right
                        hit = Physics2D.Raycast(transform.position, Vector2.right, attackRange);

                        // Flip the player to the right
                        transform.eulerAngles = new Vector3(0, 0, 0);;
                    }
                    else
                    {
                        // Shoot out a raycast to the left
                        hit = Physics2D.Raycast(transform.position, Vector2.left, attackRange);

                        // Flip the player to the left
                        transform.eulerAngles = new Vector3(0, -180, 0);
                    }
                    
                    // Check if the AI hit something
                    if (hit.collider != null)
                    {
                        // Check if the AI hit the player
                        if (hit.collider.CompareTag("Player"))
                        {
                            // Deduct health from player
                            hit.collider.gameObject.GetComponent<PlayerController>().RemoveHealth(25);
                        }
                    }
                }
                
                break;

            #endregion

            #region Dead state
            
            case AIStates.Dead:

                // Increment the timer
                deadTimer += Time.deltaTime;
                
                // Check the timer
                if (deadTimer >= deathLifetime)
                {
                    // Delete the AI
                    Destroy(gameObject);
                }
                
                break;
            
            #endregion
        }
    }

    #endregion
}
