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

    public void Start()
    {
        // Load all items from assets
        foods = LoadFoodAssets();
        tools = LoadToolAssets();
        barriers = LoadBarrierAssets();

        // set all the slots
        slots = new GameObject[slotHolder.transform.childCount];
        for (int i = 0; i < slotHolder.transform.childCount; i++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;
        }

        RefreshUI();
    }

    public void Update()
    {

    }

    public Dictionary<string, FoodClass> LoadFoodAssets()
    {
        Dictionary<string, FoodClass> foodDic = new Dictionary<string, FoodClass>();
        ItemClass[] itemAssets = Resources.LoadAll<ItemClass>("Items/Food");
        foreach (ItemClass asset in itemAssets)
        {
            FoodClass food = asset.GetFood();
            foodDic.Add(food.name, food);
        }

        return foodDic;
    }

    public Dictionary<string, ToolClass> LoadToolAssets()
    {
        Dictionary<string, ToolClass> toolDic = new Dictionary<string, ToolClass>();
        ItemClass[] itemAssets = Resources.LoadAll<ItemClass>("Items/Tool");
        foreach (ItemClass asset in itemAssets)
        {
            ToolClass tool = asset.GetTool();
            toolDic.Add(tool.name, tool);
        }

        return toolDic;
    }

    public Dictionary<string, BarrierClass> LoadBarrierAssets()
    {
        Dictionary<string, BarrierClass> barrierDic = new Dictionary<string, BarrierClass>();
        ItemClass[] itemAssets = Resources.LoadAll<ItemClass>("Items/Barrier");
        foreach (ItemClass asset in itemAssets)
        {
            BarrierClass barrier = asset.GetBarrier();
            barrierDic.Add(barrier.name, barrier);
        }

        return barrierDic;
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
                //slots[i].transform.GetChild(1).GetComponent<Image>().sprite = null;
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
