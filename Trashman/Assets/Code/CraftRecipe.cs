using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CraftRecipe  : MonoBehaviour {
    public int[] requiredItems;
    public int itemToCraft;

    public CraftRecipe(int itemToCraft, int[] requiredItems)
    {
        this.requiredItems = requiredItems;
        this.itemToCraft = itemToCraft;
    }

    void BuildCraftRecipe()
    {
      new CraftRecipe(4, new int[] {3,3,3,3});
    }
}
