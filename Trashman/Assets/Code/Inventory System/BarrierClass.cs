using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Barrier Class", menuName = "Item/Barrier")]
public class BarrierClass : ItemClass
{
    [Header("Barrier")] // data specific to barrier class

    public BarrierType barrierType;
    public enum BarrierType
    {
        Barrier, // Use tools to attack to destroy
        Trader, // Interactive obstacles that need to use Trade type items to trade with
        Monster // Monster will randomly move in the map, usually require more than one attack
    }

    public List<ToolClass> availableTools;

    public override ItemClass GetItem() { return this; }
    public override BarrierClass GetBarrier() { return this; }
    public override FoodClass GetFood() { return null; }
    public override ToolClass GetTool() { return null; }
    public override PotionClass GetPotion() { return null; }
    public override ClothesClass GetClothes() { return null; }
}