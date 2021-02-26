using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    
    #region Variables
    
    [SerializeField] private Canvas pauseCanvas;
    
    private PlayerInputs playerInputs;
    private bool isGamePaused;
    
    #endregion
    
    #region Input system

    private void Awake()
    {
        playerInputs = new PlayerInputs();
    }

    private void OnEnable()
    {
        playerInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }

    #endregion
    
    #region Start method

    private void Start()
    {
        // Disable the pause canvas by default
        pauseCanvas.enabled = false;
    }

    #endregion
    
    #region Update method

    private void Update()
    {
        // Pause game
        if (playerInputs.Player.Pause.triggered)
        {
            // Check if game is paused
            if (isGamePaused == false)
            {
                // Set the game to paused
                isGamePaused = true;
                
                // Set time scale
                Time.timeScale = 0;
                
                // Enable the canvas
                pauseCanvas.enabled = true;
            } else if (isGamePaused == true)
            {
                // Set the game to unpaused
                isGamePaused = false;
                
                // Set time scale
                Time.timeScale = 1;
                
                // Disable the canvas
                pauseCanvas.enabled = false;
            }
        }
    }

    #endregion
    
}
