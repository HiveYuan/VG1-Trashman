using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Clothes Class", menuName = "Item/Clothes")]
public class ClothesClass : ItemClass
{
    [Header("Clothes")] // data specific to clothes class

    public ClothesType clothesType;
    public enum ClothesType
    {
        Shirt
    }

    public int price;

    public override ItemClass GetItem() { return this; }
    public override ClothesClass GetClothes() { return this; }
    public override PotionClass GetPotion() { return null; }
    public override FoodClass GetFood() { return null; }
    public override ToolClass GetTool() { return null; }
    public override BarrierClass GetBarrier() { return null; }
    public override TreasureClass GetTreasure() { return null; }
}
