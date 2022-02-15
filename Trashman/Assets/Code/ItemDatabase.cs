using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemDatabase : MonoBehaviour {
    public List<Item> items = new List<Item>();

    void Awake()
    {
        BuildItemDatabase();
    }

    public Item GetItem(int id)
    {
        return items.Find(item => item.id == id);
    }

    public Item GetItem(string title)
    {
        return items.Find(item => item.title == title);
    }

    void BuildItemDatabase()
    {
        items = new List<Item>()
        {
            new Item(1, "Apple", "A delicious apple.",
            new Dictionary<string, int> {
                { "HP", 2 }
            }),
            new Item(2, "Burger", "A chessy burger.",
            new Dictionary<string, int> {
                { "HP", 5 }
            }),
            new Item(3, "Cloths", "A piece of shappy cloths, used to craft a bag.",
            new Dictionary<string, int> {

              }),
            new Item(4, "Backpack", "A 'fine' backpack, now you can store more items.",
            new Dictionary<string, int> {
                { "Slots", 5 }
            })
        };
    }
}
