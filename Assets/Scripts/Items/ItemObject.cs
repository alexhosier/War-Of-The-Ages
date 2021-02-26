using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item", menuName = "Custom/Item", order = 0)]
public class ItemObject : ScriptableObject
{
    
    [Header("Item Basic Information")] 
    public string itemName;
    public int itemId;
    public int itemQuantity;
    public int itemPrice;
    public Sprite itemImage;
    [TextArea] public string itemDescription;

    [Header("Item Use Data")] 
    public int itemHealthToGive;
    
}