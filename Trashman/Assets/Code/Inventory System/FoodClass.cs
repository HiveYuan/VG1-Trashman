using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Food Class", menuName = "Item/Food")]
public class FoodClass : ItemClass
{
    [Header("Food")] // data specific to food class

    public FoodType foodType;
    public enum FoodType
    {
        GainHealth // Use to gain health
    }

    public int bounty;
    public float healthAdded;

    public override ItemClass GetItem() { return this; }
    public override FoodClass GetFood() { return this; }
    public override ToolClass GetTool() { return null; }
    public override BarrierClass GetBarrier() { return null; }
    public override PotionClass GetPotion() { return null; }
    public override ClothesClass GetClothes() { return null; }
    public override TreasureClass GetTreasure() { return null; }
}
