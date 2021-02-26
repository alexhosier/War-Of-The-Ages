using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEditor.SceneManagement;

public class LeaderboardController : MonoBehaviour
{
    // Variables
    [SerializeField] private Transform layout;
    [SerializeField] private GameObject leaderboardItem;
    
    // Start is called before the first frame update
    void Start()
    {
        // Load the leaderboard
        StartCoroutine(LoadLeaderboard());
    }
    
    // Load leaderboard method
    IEnumerator LoadLeaderboard()
    {
        // Fetch the saves
        List<SaveSystem.SaveObject> saves = SaveSystem.FetchSaves();

        // Wait till the save files are fetch
        yield return new WaitUntil(() => saves.Count != 0);

        // Loop through each save
        foreach (SaveSystem.SaveObject save in saves)
        {
            // Create a new item
            GameObject itemCreated = Instantiate(leaderboardItem, Vector3.zero, Quaternion.identity);
        
            // Set the parent
            itemCreated.transform.SetParent(layout);
            
            // Reset size and position 
            itemCreated.transform.position = new Vector3(0, 0, 0);
            itemCreated.transform.localScale = new Vector3(1, 1, 1);
            
            // Fetch all of the text elements
            TMP_Text[] texts = itemCreated.GetComponentsInChildren<TMP_Text>();

            // Loop through the texts
            foreach (TMP_Text text in texts)
            {
                // Switch between the different values
                switch (text.name)
                {
                    // If the text is for the name
                    case "Name":

                        // Set the text to the players name
                        text.text = save.playerName;
                    
                        break;
                    
                    case "Score":

                        // Set the text to the players score
                        text.text = "Score: " + save.playerScore;
                        
                        break;
                }
            }
        }
    }
}
