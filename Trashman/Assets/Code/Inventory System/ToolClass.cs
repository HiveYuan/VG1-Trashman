using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Tool Class", menuName = "Item/Tool")]
public class ToolClass : ItemClass
{
    [Header("Tool")] // data specific to food class

    public ToolType toolType;
    public enum ToolType
    {
        knife,
        dollars
    }

    public override ItemClass GetItem() { return this; }
    public override ToolClass GetTool() { return this; }
    public override FoodClass GetFood() { return null; }
    public override BarrierClass GetBarrier() { return null; }
}
