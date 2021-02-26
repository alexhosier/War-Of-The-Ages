using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    // Variables
    public static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";

    #region Init method

    // Init save system
    public static void Init()
    {
        // If it doesn't exist
        if(!Directory.Exists(SAVE_FOLDER))
        {
            // Create the folder
            Directory.CreateDirectory(SAVE_FOLDER);
        }
    }

    #endregion

    #region Save method

    // Save functionality
    public static void Save(int playersScore) {

        // Create a new save file
        SaveObject saveObject = new SaveObject
        {
            playerName = PlayerPrefs.GetString("PlayerName"),
            playerScore = playersScore
        };

        // Encode into JSON
        string json = JsonUtility.ToJson(saveObject);

        // Write to a file
        File.WriteAllText(SAVE_FOLDER + "Save_" + saveObject.playerName + ".txt", json);
    }

    #endregion

    #region FetchSaves method
    
    // Fetch all of the saves
    public static List<SaveObject> FetchSaves()
    {
        // Init a list to send back
        List<SaveObject> saves = new List<SaveObject>();
        string[] files = Directory.GetFiles(SAVE_FOLDER, "*txt");

        // Go through each file
        foreach (string file in files)
        {
            // Check if the file exists
            if (File.Exists(file))
            {
                // Fetch the text in the file
                string saveString = File.ReadAllText(file);

                // Convert to useable data
                SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

                // Add to array
                saves.Add(saveObject);
            }
        }

        return saves;
    }
    
    #endregion

    // Save class
    public class SaveObject
    {
        public string playerName;
        public int playerScore;
    }
}