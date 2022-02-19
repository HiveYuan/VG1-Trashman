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
        apple,
        burger
    }

    public float healthAdded;

    public FoodClass(string name, FoodType type, float hp) : base(name)
    {
        itemName = name;
        foodType = type;
        healthAdded = hp;
    }

    public override ItemClass GetItem() { return this; }
    public override FoodClass GetFood() { return this; }
    public override ToolClass GetTool() { return null; }
}
