using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class InterfaceManager : MonoBehaviour
{
    // Outlets
    public GameObject storeInterface;
    public GameObject mapInterface;
    public GameObject pauseInterface;
    public GameObject settingsInterface;
    public GameObject gameManager;
    public GameController gameController;
    public InventoryManager inventory;
    public GameObject storeSlots;
    public GameObject storeItemSlot;
    public GameObject itemDetailPanel;
    public GameObject buyButton;


    // Tracking state
    GameObject currentInterface;
    GameObject currentCategory;

    // Methods
    void Awake()
    {
        gameObject.SetActive(false);
        gameController = gameManager.GetComponent<GameController>();
        itemDetailPanel.transform.GetChild(0).GetComponent<Image>().enabled = false;
        itemDetailPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().enabled = false;
        buyButton.SetActive(false);
    }

    // Move the in-game interface upward to hide any message or item box behind
    public void MoveUpwardInHierarchy()
    {
        int index = transform.GetSiblingIndex();
        transform.SetSiblingIndex(index + 1);
    }

    // Switch interface
    void SwitchInterface(GameObject requestedInterface)
    {
        currentInterface = requestedInterface;

        // Turn off all menus
        storeInterface.SetActive(false);
        mapInterface.SetActive(false);
        pauseInterface.SetActive(false);
        settingsInterface.SetActive(false);

        // Turn on requested menu
        requestedInterface.SetActive(true);
        gameObject.SetActive(true);
        MoveUpwardInHierarchy();
        gameController.DisableWholeScene();
    }



    // ------ Store interface ------
    public void ShowStoreInterface()
    {
        SwitchInterface(storeInterface);
    }

    // Switch store items category
    public void ChooseCategory()
    {
        // Clean previous category items.
        foreach (Transform item in storeSlots.transform)
        {
            Destroy(item.gameObject);
        }

        // Enable previous category button.
        if (currentCategory != null)
        {
            currentCategory.GetComponent<Button>().interactable = true;
        }

        // Get current category
        currentCategory = EventSystem.current.currentSelectedGameObject;
        string category = currentCategory.name.Split(" ")[0];

        // Set current category to disabled
        // Remain selected sprite state when choosing item under current category
        currentCategory.GetComponent<Button>().interactable = false;

        List<ItemClass> currentItems = new();
        switch (category)
        {
            case "Clothes":
                currentItems = inventory.clothes.Values.ToList<ItemClass>();
                break;
            case "Tool":
                currentItems = inventory.tools.Values.ToList<ItemClass>();
                break;
            case "Food":
                currentItems = inventory.foods.Values.ToList<ItemClass>();
                break;
            case "Potion":
                currentItems = inventory.potions.Values.ToList<ItemClass>();
                break;
            case "Barrier":
                currentItems = inventory.barriers.Values.ToList<ItemClass>();
                break;
            default:
                break;
        }

        foreach (ItemClass item in currentItems)
        {
            GameObject itemSlot = Instantiate(storeItemSlot, storeSlots.transform);
            itemSlot.transform.GetChild(0).GetComponent<Image>().sprite = item.itemIcon;
            itemSlot.GetComponent<Button>().onClick.AddListener(ChooseItem);
            itemSlot.name = category + "-" + item.name;
        }
    }

    // Show selected item detail information
    public void ChooseItem()
    {
        string[] objName = EventSystem.current.currentSelectedGameObject.name.Split("-");
        string category = objName[0];
        string name = objName[1];

        ItemClass item = null;
        string introText = "";
        buyButton.SetActive(true);
        buyButton.GetComponentInChildren<TMP_Text>().text = "Buy";

        switch (category)
        {
            case "Clothes":
                item = inventory.clothes[name];
                introText += "Type: <gradient=GoldWhite>" + item.GetClothes().clothesType + "</gradient>\n";
                introText += "Price: <gradient=GoldWhite>" + item.GetClothes().price + "</gradient>\n";
                if (item.GetClothes().price == 0)
                {
                    buyButton.GetComponentInChildren<TMP_Text>().text = "Free";
                }
                buyButton.SetActive(true);
                break;
            case "Tool":
                item = inventory.tools[name];
                introText += "Type: <gradient=GoldWhite>" + item.GetTool().toolType + "</gradient>\n";
                buyButton.SetActive(false);
                break;
            case "Food":
                item = inventory.foods[name];
                introText += "Type: <gradient=GoldWhite>" + item.GetFood().foodType + "</gradient>\n";
                introText += "Gain HP: <gradient=GoldWhite>" + item.GetFood().healthAdded + "</gradient>\n";
                buyButton.SetActive(false);
                break;
            case "Potion":
                item = inventory.potions[name];
                introText += "Type: <gradient=GoldWhite>" + item.GetPotion().potionType + "</gradient>\n";
                if (item.GetPotion().potionType == PotionClass.PotionType.LuckyPower)
                {
                    introText += "Gain Lucky: <gradient=GoldWhite>" + item.GetPotion().lucky *100 + "%</gradient>\n";
                }
                introText += "Price: <gradient=GoldWhite>" + item.GetPotion().price + "</gradient>\n";
                buyButton.SetActive(true);
                break;
            case "Barrier":
                item = inventory.barriers[name];
                introText += "Type: <gradient=GoldWhite>" + item.GetBarrier().barrierType + "</gradient>\n";
                if (item.GetBarrier().barrierType == BarrierClass.BarrierType.Trader)
                {
                    introText += "What does it want:";
                }
                else
                {
                    introText += "How To Destroy:";
                }
                introText += " <gradient=GoldWhite>" + string.Join(' ', item.GetBarrier().getToolNameList()) + "</gradient>\n";
                buyButton.SetActive(false);
                break;
            default:
                break;
        }

        itemDetailPanel.transform.GetChild(0).GetComponent<Image>().enabled = true;
        itemDetailPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().enabled = true;
        itemDetailPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = item.itemIcon;
        if (PlayerPrefs.GetInt(name+"_new") == 1)
        {
            itemDetailPanel.transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>().text = "?";
            itemDetailPanel.transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>().alignment = TextAlignmentOptions.Center;
            buyButton.SetActive(false);
        }
        else
        {
            string basicText = "Item Name: <gradient=GoldWhite>" + item.name + "</gradient>\n" +
                "Item Intro: <gradient=GoldWhite>" + item.itemIntro + "</gradient>\n";
            itemDetailPanel.transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>().text =  basicText + introText;
            itemDetailPanel.transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>().alignment = TextAlignmentOptions.Left;
        }
    }

    // TODO: Buy item: where to store the item?
    public void BuyItem()
    {

    }



    // ------ Pause interface ------
    public void ShowPauseInterface()
    {
        SwitchInterface(pauseInterface);
    }

    // Close current interface and continue playing
    public void Continue()
    {
        currentInterface.SetActive(false);
        gameController.EnableWholeScene();
    }

    // Reload current level
    public void Reload()
    {
        currentInterface.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Quit current level and go to the start menu scene
    public void Quit()
    {
        SceneManager.LoadScene("StartMenu");
    }



    // TODO: Load map interface / Backpack interface?
    public void ShowMapInterface()
    {
        SwitchInterface(mapInterface);
    }

    // TODO: Load settings interface
    public void ShowSettingsInterface()
    {
        SwitchInterface(settingsInterface);
    }
}
