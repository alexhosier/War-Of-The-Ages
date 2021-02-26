using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    #region Variables

    // Variables
    [Header("UI References")]
    [SerializeField] private Image[] hearts;
    [SerializeField] private TMP_Text scoreText;

    #endregion

    #region UpdateHealthUI method
    
    // Update health method
    public void UpdateHealthUI(int health)
    {
        // Switch between the health values
        switch (health)
        {
            // When the player is at max health
            case 100:

                // Enable all of the images
                foreach (Image image in hearts)
                {
                    image.enabled = true;
                }
                
                break;
            
            // When the player is at three thirds health
            case 75:

                // Enable the first three hearts
                hearts[0].enabled = true;
                hearts[1].enabled = true;
                hearts[2].enabled = true;
                hearts[3].enabled = false;
                
                break;
            
            // When the player is at half health
            case 50:

                // Enable the first two hearts
                hearts[0].enabled = true;
                hearts[1].enabled = true;
                hearts[2].enabled = false;
                hearts[3].enabled = false;
                
                break;
            
            // When the player is at quarter health
            case 25:

                // Enable the first heart
                hearts[0].enabled = true;
                hearts[1].enabled = false;
                hearts[2].enabled = false;
                hearts[3].enabled = false;

                break;
        }
    }
    
    #endregion

    #region UpdateScoreUI method

    // Update score UI 
    public void UpdateScoreUI(int score)
    {
        scoreText.text = "Score: " + score;
    }

    #endregion
    
}
