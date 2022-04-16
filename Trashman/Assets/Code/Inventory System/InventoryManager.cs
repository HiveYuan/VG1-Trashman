using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject slotHolder;
    public List<SlotClass> items = new();
    private GameObject[] slots;

    public Dictionary<string, FoodClass> foods;
    public Dictionary<string, ToolClass> tools;
    public Dictionary<string, BarrierClass> barriers;
    public Dictionary<string, PotionClass> potions;
    public Dictionary<string, ClothesClass> clothes;

    public void Start()
    {
        // Load all items from assets
        foods = LoadFoodAssets();
        tools = LoadToolAssets();
        barriers = LoadBarrierAssets();
        potions = LoadPotionAssets();
        clothes = LoadClothesAssets();

        // set all the slots
        slots = new GameObject[slotHolder.transform.childCount];
        for (int i = 0; i < slotHolder.transform.childCount; i++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;
        }

        RefreshUI();
    }
    

    public Dictionary<string, FoodClass> LoadFoodAssets()
    {
        Dictionary<string, FoodClass> foodDic = new Dictionary<string, FoodClass>();
        FoodClass[] foodAssets = Resources.LoadAll<FoodClass>("Items/Food");
        foreach (FoodClass food in foodAssets)
        {
            foodDic.Add(food.name, food);
            if (!PlayerPrefs.HasKey("AlreadyLoad"))
            {
                PlayerPrefs.SetInt(food.name + "_new", 1);
            }
        }
        return foodDic;
    }

    public Dictionary<string, ToolClass> LoadToolAssets()
    {
        Dictionary<string, ToolClass> toolDic = new Dictionary<string, ToolClass>();
        ToolClass[] toolAssets = Resources.LoadAll<ToolClass>("Items/Tool");
        foreach (ToolClass tool in toolAssets)
        {
            toolDic.Add(tool.name, tool);
            if (!PlayerPrefs.HasKey("AlreadyLoad"))
            {
                PlayerPrefs.SetInt(tool.name + "_new", 1);
            }
        }
        return toolDic;
    }

    public Dictionary<string, BarrierClass> LoadBarrierAssets()
    {
        Dictionary<string, BarrierClass> barrierDic = new Dictionary<string, BarrierClass>();
        BarrierClass[] barrierAssets = Resources.LoadAll<BarrierClass>("Items/Barrier");
        foreach (BarrierClass barrier in barrierAssets)
        {
            barrierDic.Add(barrier.name, barrier);
            if (!PlayerPrefs.HasKey("AlreadyLoad"))
            {
                PlayerPrefs.SetInt(barrier.name + "_new", 1);
            }
        }
        return barrierDic;
    }

    public Dictionary<string, PotionClass> LoadPotionAssets()
    {
        Dictionary<string, PotionClass> potionDic = new Dictionary<string, PotionClass>();
        PotionClass[] potionAssets = Resources.LoadAll<PotionClass>("Items/Potion");
        foreach (PotionClass potion in potionAssets)
        {
            potionDic.Add(potion.name, potion);
            if (!PlayerPrefs.HasKey("AlreadyLoad"))
            {
                PlayerPrefs.SetInt(potion.name + "_new", 0);
            }
        }
        return potionDic;
    }

    public Dictionary<string, ClothesClass> LoadClothesAssets()
    {
        Dictionary<string, ClothesClass> clothesDic = new Dictionary<string, ClothesClass>();
        ClothesClass[] clothesAssets = Resources.LoadAll<ClothesClass>("Items/Clothes");
        foreach (ClothesClass clothes in clothesAssets)
        {
            clothesDic.Add(clothes.name, clothes);
            if (!PlayerPrefs.HasKey("AlreadyLoad"))
            {
                PlayerPrefs.SetInt(clothes.name + "_new", 0);
            }
        }
        PlayerPrefs.SetInt("AlreadyLoad", 1);
        return clothesDic;
    }

    public void RefreshUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            try
            {
                slots[i].transform.GetChild(1).GetComponent<Image>().enabled = true;
                slots[i].transform.GetChild(1).GetComponent<Image>().sprite = items[i].GetItem().itemIcon;
                slots[i].transform.GetChild(2).GetComponent<Image>().enabled = true;
                slots[i].transform.GetChild(3).GetComponent<TMP_Text>().text = items[i].GetQuantity() + "";

            }
            catch
            {
                slots[i].transform.GetChild(1).GetComponent<Image>().enabled = false;
                slots[i].transform.GetChild(2).GetComponent<Image>().enabled = false;
                slots[i].transform.GetChild(3).GetComponent<TMP_Text>().text = "";
            }
        }
    }

    public void Add(ItemClass item)
    {
        // check if inventory contains this item
        SlotClass slot = Contains(item);
        if (slot != null)
        {
            slot.AddQuantity(1);
        }
        else
        {
            items.Add(new SlotClass(item, 1));
        }

        RefreshUI();
    }

    public ItemClass Remove(int idx)
    {
        ItemClass item = null;
        if (items.Count >= idx)
        {
            SlotClass slot = items[idx - 1];
            item = slot.GetItem();
            if (slot.GetQuantity() > 1)
            {
                slot.SubQuantity(1);
            }
            else if (slot.GetQuantity() == 1)
            {
                items.Remove(slot);
            }
        }
        else
        {
            //TODO: Error handling, player try to use non-existing item
            return item;
        }

        RefreshUI();
        return item;
    }

    public SlotClass Contains(ItemClass item)
    {
        foreach(SlotClass slot in items)
        {
            if (slot.GetItem() == item)
            {
                return slot;
            }
        }
        return null;
    }

    public ItemClass Get(int idx)
    {
        ItemClass item = null;
        if (items.Count >= idx)
        {
            SlotClass slot = items[idx - 1];
            item = slot.GetItem();
        }

        RefreshUI();
        return item;
    }
}
