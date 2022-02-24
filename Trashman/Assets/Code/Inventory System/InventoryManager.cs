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

    ItemClass apple;
    ItemClass burger;
    ItemClass knife;

    public void Start()
    {
        apple = Resources.Load<ItemClass>("Classes/WholeApple");
        burger = Resources.Load<ItemClass>("Classes/WholeBurger");
        knife = Resources.Load<ItemClass>("Classes/WhiteKnife");

        slots = new GameObject[slotHolder.transform.childCount];
        // set all the slots
        for (int i = 0; i < slotHolder.transform.childCount; i++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;
        }

        RefreshUI();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Add(apple);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Add(burger);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Add(knife);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Remove(apple);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Remove(burger);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Remove(knife);
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
