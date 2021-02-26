using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    
    #region Variables

    [Header("Inventory Settings")]
    [SerializeField] private int columnLength;
    [SerializeField] private int rowsHeight;
    [SerializeField] private ItemObject tempItem;

    [Header("GameObject References")] 
    [SerializeField] private Canvas inventoryCanvas;
    [SerializeField] private Sprite defaultIcon;
    [SerializeField] private Transform inventoryParent;
    [SerializeField] private GameObject invetoryItem;
    
    private ItemObject[,] inventory;
    private PlayerInputs playerInputs;
    private bool isInventoryOpened;

    #endregion

    #region Start method

    void Start()
    {
        // Init array
        inventory = new ItemObject[columnLength, rowsHeight];
        
        // Go through the columns
        for (int c = 0; c < columnLength; c++)
        {
            // Go through the rows
            for (int r = 0; r < rowsHeight; r++)
            {
                inventory[c, r] = tempItem;
            }
        }
        
        // Hide the canvas by default
        inventoryCanvas.enabled = false;
    }

    #endregion

    #region Awake method

    // Called when the script is loaded
    private void Awake()
    {
        // Create a new instance of the PlayerInputs
        playerInputs = new PlayerInputs();
    }

    #endregion
    
    #region PlayerInputs

    // When the class is enabled
    private void OnEnable()
    {
        // Enable the instance
        playerInputs.Enable();
    }

    // When the class is disabled
    private void OnDisable()
    {
        // Disable the instance
        playerInputs.Disable();
    }

    #endregion

    #region Update method

    // Called once per frame
    private void Update()
    {
        // If player presses the inventory button and it is not open
        if (playerInputs.Player.Inventory.triggered && !isInventoryOpened)
        {
            // Fetch all of the items
            Transform[] items = inventoryParent.GetComponentsInChildren<Transform>();

            // Go through all of the images
            foreach (Transform item in items)
            {
                // Check if it is an InventoryItem
                if (item.name.Contains("InventoryItem"))
                {
                    // Destroy the item
                    Destroy(item.gameObject);
                }
            }
            
            // Loop through all of the columns
            for (int c = 0; c < columnLength; c++)
            {
                // Loop through all of the rows
                for (int r = 0; r < rowsHeight; r++)
                {
                    // Check if the items is not a temp item
                    if (inventory[c, r].itemId != 0)
                    {
                        // Create a new item in the inventory GUI
                        GameObject itemCreated = Instantiate(invetoryItem, Vector3.zero, Quaternion.identity);
                        
                        // Set the parent to the layout
                        itemCreated.transform.SetParent(inventoryParent.transform);
                        
                        // Reset the position and scale
                        itemCreated.transform.position = Vector3.zero;
                        itemCreated.transform.localScale = new Vector3(1, 1, 1);
                        
                        // Fetch all of the images in the children
                        Image[] images = itemCreated.GetComponentsInChildren<Image>();

                        // Loop the images found
                        foreach (Image image in images)
                        {
                            // If it is the correct image
                            if (image.gameObject.name == "Icon")
                            {
                                // Check if the item has an image
                                if (inventory[c, r].itemImage != null)
                                {
                                    // Set the image to the correct sprite
                                    image.sprite = inventory[c, r].itemImage;
                                }
                                else
                                {
                                    // Set the image to the default sprite
                                    image.sprite = defaultIcon;
                                }
                            }
                        }

                        // Fetch all of the Text in the children
                        Text[] texts = itemCreated.GetComponentsInChildren<Text>();

                        // Loop through all of the found Text
                        foreach (Text text in texts)
                        {
                            // Switch for name and quantity
                            switch (text.name)
                            {
                                // The name of the item
                                case "Name":

                                    // Set the name of the item
                                    text.text = inventory[c, r].itemName;
                                    
                                    break;
                                
                                // The quantity of the item
                                case "Quantity":

                                    // Set the amount of bread
                                    text.text = "" + inventory[c, r].itemQuantity;
                                    
                                    break;
                            }
                        }
                    }
                }
            }

            // Set the player in the inventory
            isInventoryOpened = true;
            
            // Show the canvas GUI
            inventoryCanvas.enabled = true;
        } else if (playerInputs.Player.Inventory.triggered && isInventoryOpened)
        {
            // Set the player out of the inventory
            isInventoryOpened = false;

            // Close the canvas GUI
            inventoryCanvas.enabled = false;
        }
    }

    #endregion
    
    #region AddItem method

    public void AddItem(ItemObject itemToAdd, int itemQuantityToAdd)
    {
        // Go through the columns
        for (int c = 0; c < columnLength; c++)
        {
            // Go through the rows
            for (int r = 0; c < rowsHeight; r++)
            {
                // Check if the item is already in the inventory
                if (inventory[c, r].itemId == itemToAdd.itemId)
                {
                    // Increment the quantity of the item
                    inventory[c, r].itemQuantity += itemQuantityToAdd;

                    // Stop it from progressing
                    return;
                }
                
                // Check if the slot is not taken
                if (inventory[c, r] == tempItem)
                {
                    // Replace the temp item with the new item
                    inventory[c, r] = itemToAdd;
                    
                    // Add the quantity 
                    inventory[c, r].itemQuantity = itemQuantityToAdd;

                    // Stop it from progressing
                    return;
                }
            }
        }
    }

    #endregion

    #region RemoveItem method

    public void RemoveItem(ItemObject itemToRemove)
    {
        // Go through the columns
        for (int c = 0; c < columnLength; c++)
        {
            // Go through the rows
            for (int r = 0; c < rowsHeight; r++)
            {
                // If the row matches the item to remove
                if (inventory[c, r] == itemToRemove)
                {
                    // Replace with temp item
                    inventory[c, r] = tempItem;
                }
            }
        }
    }

    #endregion
    
}
