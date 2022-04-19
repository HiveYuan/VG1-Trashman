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
        Barrier, // Use tools to attack to destroy, will drop treasure
        Trader, // Interactive obstacles that need to use Trade type items to trade with, will NOT drop treasure
        Monster // Monster will randomly move in the map, usually require more than one attack, will drop treasure
    }

    [Header("Type: Barrier/Monster")]
    public int hp;
    public List<ToolClass> availableTools;
    public List<string> getToolNameList()
    {
        List<string> toolNames = new();
        foreach (ToolClass tool in this.availableTools)
        {
            toolNames.Add(tool.name);
        }
        return toolNames;
    }
    public List<float> dropTreasureProbs; // Sum of all the probs must <= 1
    public List<TreasureClass> dropTreasures;
    public List<string> getDropNameList()
    {
        List<string> treasureNames = new();
        foreach (TreasureClass treasure in this.dropTreasures)
        {
            treasureNames.Add(treasure.name);
        }
        return treasureNames;
    }


    [Header("Type: Trader")]
    public List<TreasureClass> availableTreasures;
    public List<string> getTreasureNameList()
    {
        List<string> treasureNames = new();
        foreach (TreasureClass treasure in this.availableTreasures)
        {
            treasureNames.Add(treasure.name);
        }
        return treasureNames;
    }

    public override ItemClass GetItem() { return this; }
    public override BarrierClass GetBarrier() { return this; }
    public override FoodClass GetFood() { return null; }
    public override ToolClass GetTool() { return null; }
    public override PotionClass GetPotion() { return null; }
    public override ClothesClass GetClothes() { return null; }
    public override TreasureClass GetTreasure() { return null; }
}