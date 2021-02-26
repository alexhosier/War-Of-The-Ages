using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerName : MonoBehaviour
{
    [SerializeField] private GameObject playerScreen;
    [SerializeField] private InputField playerName;

    // Start is called before the first frame update
    void Start()
    {
        // Check if it exists
        if(!PlayerPrefs.HasKey("PlayerName"))
        {
            // Show the player name screen
            playerScreen.SetActive(true);
        } else
        {
            // Hide the player name screen
            playerScreen.SetActive(false);
        }
    }

    // Update the players name in settings
    public void UpdateName()
    {
        // Update value
        PlayerPrefs.SetString("PlayerName", playerName.text);

        // Hide the screen
        playerScreen.SetActive(false);
    }
}