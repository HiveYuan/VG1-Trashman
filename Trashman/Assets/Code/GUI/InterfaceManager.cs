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
        itemDetailPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().enabled = false;
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
                //inventory.clothes;
                break;
            case "Tool":
                currentItems = inventory.tools.Values.ToList<ItemClass>();
                break;
            case "Food":
                currentItems = inventory.foods.Values.ToList<ItemClass>();
                break;
            case "Potion":
                //inventory.potions;
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
    // TODO: add price?
    // TODO: which condition need buy button?
    public void ChooseItem()
    {
        string[] objName = EventSystem.current.currentSelectedGameObject.name.Split("-");
        string category = objName[0];
        string name = objName[1];

        ItemClass item = null;
        string introText = "";
        buyButton.SetActive(true);

        switch (category)
        {
            case "Clothes":
                // TODO: add clothes
                break;
            case "Tool":
                item = inventory.tools[name];
                introText += "Function Type: " + item.GetTool().toolType + "\n";
                break;
            case "Food":
                item = inventory.foods[name];
                introText += "Function Type: " + item.GetFood().foodType + "\n";
                introText += "Function Type: " + item.GetFood().healthAdded + "\n";
                break;
            case "Potion":
                // TODO: add potion
                break;
            case "Barrier":
                item = inventory.barriers[name];
                introText += "Function Type: " + item.GetBarrier().barrierType + "\n";
                introText += "How To Destroy: " + item.GetBarrier().availableTools.ToString() + "\n";
                buyButton.SetActive(false);
                break;
            default:
                break;
        }

        itemDetailPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().enabled = true;
        itemDetailPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = item.itemIcon;
        if (item.isFirstTime)
        {
            itemDetailPanel.transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>().text = "?";
            itemDetailPanel.transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>().fontSize = 108;
            buyButton.SetActive(false);
        }
        else
        {
            string basicText = "Item Name: " + item.name + "\n" + "Item Intro: " + item.itemIntro + "\n";
            itemDetailPanel.transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>().text =  basicText + introText;
            itemDetailPanel.transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>().fontSize = 18;
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
