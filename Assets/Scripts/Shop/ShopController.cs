using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{

    #region Variables

    // Variables
    [Header("Referencing Objects")]
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private ItemObject[] itemsAvailable;
    
    [Header("Referencing UI Elements")]
    [SerializeField] private Canvas shopCanvas;
    [SerializeField] private GameObject shopItemTemplate;
    [SerializeField] private GameObject shopItemLayout;
    [SerializeField] private Sprite shopItemIconDefault;

    #endregion
    
    #region Start method
    
    // Start method
    private void Start()
    {
        // Fetch the playerInventory class from the player object
        playerInventory = GameObject.FindWithTag("Player").GetComponent<PlayerInventory>();
        
        // Close the shop by default
        shopCanvas.enabled = false;
    }
    
    #endregion
    
    #region OpenShop method

    // Function to open the shop
    public void OpenShop()
    {
        // Fetch all of the items
        Transform[] items = shopItemLayout.GetComponentsInChildren<Transform>();

        // Go through all of the images
        foreach (Transform item in items)
        {
            // Check if it is an InventoryItem
            if (item.name.Contains("ShopItem"))
            {
                // Destroy the item
                Destroy(item.gameObject);
            }
        }

        // Foreach item that is for sale
        foreach (ItemObject item in itemsAvailable)
        {
            // Create the item
            GameObject itemCreated = Instantiate(shopItemTemplate, Vector3.zero, Quaternion.identity);
            
            // Set the parent to the layout
            itemCreated.transform.SetParent(shopItemLayout.transform);
            
            // Reset the position and scale
            itemCreated.transform.position = Vector3.zero;
            itemCreated.transform.localScale = new Vector3(1, 1, 1);
            
            // Variable for texts in the shop item template
            Text[] texts = itemCreated.GetComponentsInChildren<Text>();

            // Go through the texts
            foreach (Text text in texts)
            {
                // If the text is for the name
                switch (text.name)
                {
                    // If the text is the item name
                    case "ShopItemName":

                        // Set the name of the item
                        text.text = item.itemName;
                        
                        break;
                    
                    // If the text is the item price
                    case "ShopItemPrice":

                        // Set the price of the item
                        text.text = item.itemPrice + " Gold";
                        
                        break;
                }
            }

            // Get all of the images
            Image[] images = itemCreated.GetComponentsInChildren<Image>();
            Image imageToChange = null;
            
            // Loop through all of the images
            foreach (Image image in images)
            {
                // If it is the correct image
                if (image.name == "ShopItemImage")
                {
                     imageToChange = image;
                }
            }
            
            // Set item image
            if (item.itemImage == null)
            {
                // Set the image to a placeholder image
                imageToChange.sprite = shopItemIconDefault;
            }
            else
            {
                // Set it to the icon
                imageToChange.sprite = item.itemImage;    
            }
            
            // Fetch the buy button
            Button buyButton = itemCreated.GetComponentInChildren<Button>();
            
            // BUG Gives the player three of each items
            
            // Attach functionality to it
            buyButton.onClick.AddListener(delegate {BuyItem(item.itemId, 1);});
        }
        
        // Show the shop GUI
        shopCanvas.enabled = true;
    }

    #endregion

    #region CloseShop method

    public void CloseShop()
    {
        // Hide the shop GUI
        shopCanvas.enabled = false;
    }

    #endregion

    #region BuyItem method

    // Function to buy items
    public void BuyItem(int itemID, int itemQuantity)
    {
        // Go through all of the items
        foreach (ItemObject item in itemsAvailable)
        {
            // Check if it is the item we are looking for
            if (item.itemId == itemID)
            {
                // Add the item to the inventory
                playerInventory.AddItem(item, itemQuantity);
            }    
        }
    }

    #endregion
    
    #region SellItemMethod
    
    // Function to sell items
    public void SellItem(int itemID)
    {
        // Compensate the player for the item
        foreach (ItemObject item in itemsAvailable)
        {
            // Check if the item is gold
            if (item.itemName == "Gold")
            {
                // Give the player 10 gold for each item
                playerInventory.AddItem(item, 10);
            }
        }
        
        // Remove the old item
        foreach (ItemObject item in itemsAvailable)
        {
            // Check if it is the item the player is selling
            if (item.itemId == itemID)
            {
                // Remove the item from the players inventory
                playerInventory.RemoveItem(item);
            }
        }
    }
    
    #endregion
    
}
