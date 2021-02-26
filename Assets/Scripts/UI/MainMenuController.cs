using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    
    #region Variables

    private enum MenuOptions { StartGame, Options, ExitGame }

    [Header("UI Options")]
    [SerializeField] private MenuOptions selectedOption;

    [Header("UI References")]
    [SerializeField] private Image[] startImages;
    [SerializeField] private Image[] optionImages;
    [SerializeField] private Image[] exitImages;
    [SerializeField] private Canvas optionsCanvas;

    private PlayerInputs playerInputs;
    private bool inMenu;

    #endregion
    
    #region Player input system
    
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

    #region Start method

    private void Start()
    {
        // Set initial point
        selectedOption = MenuOptions.StartGame;

        // Disable non relevant images
        foreach(Image image in optionImages) {
            image.enabled = false;
        }

        foreach(Image image in exitImages) {
            image.enabled = false;
        }
        
        // Hide the options canvas
        optionsCanvas.enabled = false;
    }

    #endregion

    #region Update method

    private void Update()
    {
        // If the player presses up in UI
        if(playerInputs.UI.NavigateUp.triggered && selectedOption != MenuOptions.StartGame && !inMenu) {
            selectedOption -= 1;

            UpdateSelection();
        }

        // If the player presses down in UI
        if(playerInputs.UI.NavigateDown.triggered && selectedOption != MenuOptions.ExitGame && !inMenu) {
            selectedOption += 1;

            UpdateSelection();
        }

        // If the player enters a menu
        if (playerInputs.UI.NavigateEnter.triggered)
        {
            // Switch between options
            switch (selectedOption)
            {
                // Start game
                case MenuOptions.StartGame:
                    // Call LoadScene method with parameter 1
                    LoadScene(1);
                    break;
                
                // Options menu
                case MenuOptions.Options:
                    // Open the options menu
                    OpenOptions();
                    break;
                
                // Close game
                case MenuOptions.ExitGame:
                    // Close the application
                    CloseGame();
                    break;
            }
        }
    }

    #endregion

    #region UpdateSelection method

    private void UpdateSelection() {
        // Switch between the options
        switch (selectedOption)
        {
            // Start game
            case MenuOptions.StartGame:

                // Disable old images
                foreach(Image image in optionImages) {
                    image.enabled = false;
                }

                foreach(Image image in exitImages) {
                    image.enabled = false;
                }

                // Enable the new images
                foreach(Image image in startImages) {
                    image.enabled = true;
                }

                break;

            // Options
            case MenuOptions.Options:

                // Disable old images
                foreach (Image image in startImages)
                {
                    image.enabled = false;
                }

                foreach (Image image in exitImages)
                {
                    image.enabled = false;
                }

                // Enable the new images
                foreach (Image image in optionImages)
                {
                    image.enabled = true;
                }

                break;

            // Exit game
            case MenuOptions.ExitGame:

                // Disable old images
                foreach (Image image in optionImages)
                {
                    image.enabled = false;
                }

                foreach (Image image in startImages)
                {
                    image.enabled = false;
                }

                // Enable the new images
                foreach (Image image in exitImages)
                {
                    image.enabled = true;
                }

                break;
        }
    }

    #endregion

    #region Selection functionality methods

    // Load scene from int
    private void LoadScene(int sceneToLoad) {
        // Load scene
        SceneManager.LoadScene(sceneToLoad);
    }

    // Open the options menu
    private void OpenOptions()
    {
        // Show the canvas
        optionsCanvas.enabled = true;
        
        // Set player in a menu
        inMenu = true;
    }

    // Close the options menu
    public void CloseOptions()
    {
        // Hide the canvas
        optionsCanvas.enabled = false;
        
        // Set player in a menu
        inMenu = false;
    }

    // Quit the game
    private void CloseGame()
    {
        // Close the application
        Application.Quit(0);
    }

    #endregion
    
}
