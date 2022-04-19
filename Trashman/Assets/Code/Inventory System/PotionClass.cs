using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Potion Class", menuName = "Item/Potion")]
public class PotionClass : ItemClass
{
    [Header("Potion")] // data specific to potion class

    public PotionType potionType;
    public enum PotionType
    {
        DamagePower, // Use to gain damage buff (won't upgrade attack range, won't effect trade tools)
        RangePower, // Use to gain attack range buff (won't upgrade attack damage, won't effect trade tools)
        PickPower, // Pick more items
        LuckyPower // Upgrade possibility to drop items after destroy barriers
    }

    public float lucky;
    public int buff;
    public int price;

    public override ItemClass GetItem() { return this; }
    public override PotionClass GetPotion() { return this; }
    public override FoodClass GetFood() { return null; }
    public override ToolClass GetTool() { return null; }
    public override BarrierClass GetBarrier() { return null; }
    public override ClothesClass GetClothes() { return null; }
    public override TreasureClass GetTreasure() { return null; }
}
