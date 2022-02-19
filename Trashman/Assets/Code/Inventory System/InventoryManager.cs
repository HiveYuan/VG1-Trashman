using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject slotHolder;
    [SerializeField] private ItemClass itemToAdd;
    [SerializeField] private ItemClass itemToRemove;

    public List<SlotClass> items = new();

    private GameObject[] slots;

    public void Start()
    {
        slots = new GameObject[slotHolder.transform.childCount];
        // set all the slots
        for (int i = 0; i < slotHolder.transform.childCount; i++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;
        }

        Add(itemToAdd);
        Remove(itemToRemove);

        RefreshUI();
    }

    public void Update()
    {
        // TODO: add items from script using keyboard but not initialize from Unity UI
        // TODO: how to refer custom class asset?
        for (int i = 1; i < 1+ items.Count; i++)
        {
            if (Input.GetKeyDown("" + i))
            {
                Add(items[i - 1].GetItem());
            }
        }
        for (int i = 1+ items.Count; i < 1 + items.Count + items.Count; i++)
        {
            if (Input.GetKeyDown("" + i))
            {
                Remove(items[i - 1 - items.Count].GetItem());
            }
        }
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
                slots[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = items[i].GetQuantity() + "";

            }
            catch
            {
                slots[i].transform.GetChild(1).GetComponent<Image>().enabled = false;
                //slots[i].transform.GetChild(1).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(2).GetComponent<Image>().enabled = false;
                slots[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "";
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

    public bool Remove(ItemClass item)
    {
        // check if inventory contains this item
        SlotClass slot = Contains(item);
        if (slot != null)
        {
            if (slot.GetQuantity() > 1)
            {
                slot.SubQuantity(1);
            }
            else
            {
                items.Remove(slot);
            }
        }
        else
        {
            //TODO: Error handling, player try to use non-existing item
            return false;
        }

        RefreshUI();
        return true;
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
}
