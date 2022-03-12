using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemClass : ScriptableObject
{
    [Header("Item")] // data shared across every item
    public string itemName;
    public Sprite itemIcon;
    public string itemIntro;
    public string itemType;
    public bool isFirstTime;

    public abstract ItemClass GetItem();
    public abstract FoodClass GetFood();
    public abstract ToolClass GetTool();
}
