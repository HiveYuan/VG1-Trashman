using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Treasure Class", menuName = "Item/Treasure")]
public class TreasureClass : ItemClass
{
    [Header("Treasure")] // data specific to treasure class

    public TreasureType treasureType;
    public enum TreasureType
    {
        Trade // Use to trade with any interactive obstacle
    }

    public override ItemClass GetItem() { return this; }
    public override TreasureClass GetTreasure() { return this; }
    public override ToolClass GetTool() { return null; }
    public override FoodClass GetFood() { return null; }
    public override BarrierClass GetBarrier() { return null; }
    public override PotionClass GetPotion() { return null; }
    public override ClothesClass GetClothes() { return null; }
}
