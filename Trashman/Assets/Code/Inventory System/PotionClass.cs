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
        DoubleDamagePower, // Use to gain double double damage (won't double attack range)
        DoubleRangePower, // Use to gain double double attack range (won't double attack damage)
        DoublePickPower, // Pick double items
        LuckyPower // Upgrade possibility to drop items after destroy any type of barriers
    }

    public float lucky;
    public int price;

    public override ItemClass GetItem() { return this; }
    public override PotionClass GetPotion() { return this; }
    public override FoodClass GetFood() { return null; }
    public override ToolClass GetTool() { return null; }
    public override BarrierClass GetBarrier() { return null; }
    public override ClothesClass GetClothes() { return null; }
}
