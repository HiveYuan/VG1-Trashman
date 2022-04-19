using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemClass : ScriptableObject
{
    [Header("Item")] // data shared across every item
    public Sprite itemIcon;
    public string itemIntro;

    public abstract ItemClass GetItem();
    public abstract FoodClass GetFood();
    public abstract ToolClass GetTool();
    public abstract BarrierClass GetBarrier();
    public abstract PotionClass GetPotion();
    public abstract ClothesClass GetClothes();
    public abstract TreasureClass GetTreasure();
}
