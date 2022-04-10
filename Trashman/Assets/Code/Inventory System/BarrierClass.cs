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
        security,
        wood
    }

    public List<ToolClass> availableTools;

    public override ItemClass GetItem() { return this; }
    public override BarrierClass GetBarrier() { return this; }
    public override FoodClass GetFood() { return null; }
    public override ToolClass GetTool() { return null; }
}