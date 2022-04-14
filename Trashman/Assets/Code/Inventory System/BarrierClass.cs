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
        Trade // Interactive obstacles that need to use Trade type items to trade with
    }

    public List<ToolClass> availableTools;

    public override ItemClass GetItem() { return this; }
    public override BarrierClass GetBarrier() { return this; }
    public override FoodClass GetFood() { return null; }
    public override ToolClass GetTool() { return null; }
}