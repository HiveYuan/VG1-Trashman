using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using Trashman;

public class InterfaceManager : MonoBehaviour
{
    // Outlets
    public GameObject storeInterface;
    public GameObject collectionInterface;
    public GameObject pauseInterface;
    public GameObject settingsInterface;
    public GameObject gameManager;
    public GameController gameController;
    public InventoryManager inventory;
    public PlayerController playerController;
    SoundManager soundManager;

    [Header("Store")]
    // store interface outlets
    public GameObject storeSlots;
    public GameObject storeItemSlot;
    public GameObject storeItemDetailPanel;
    public GameObject buyButton;
    public TMP_Text coinPrompt;

    [Header("Collection")]
    // collection interface outlets
    public GameObject collectionSlots;
    public GameObject collectionItemSlot;
    public GameObject collectionItemDetailPanel;
    public GameObject useButton;

    [Header("Settings")]
    // settings interface outlets
    public Button soundButton;
    public Sprite soundEnable;
    public Sprite soundDisable;


    // Tracking state
    GameObject currentInterface;
    // Tracking store state
    GameObject currentStoreCategory;
    string currentStoreItemName = "";
    // Tracking collection state
    GameObject currentCollectionCategory;
    string currentCollectionItemName = "";

    // Methods
    void Awake()
    {
        gameObject.SetActive(false);
        gameController = gameManager.GetComponent<GameController>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        // Store
        storeItemDetailPanel.transform.GetChild(0).GetComponent<Image>().enabled = false;
        storeItemDetailPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().enabled = false;
        buyButton.SetActive(false);
        coinPrompt.enabled = false;
        // Collection
        collectionItemDetailPanel.transform.GetChild(0).GetComponent<Image>().enabled = false;
        collectionItemDetailPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().enabled = false;
        useButton.SetActive(false);
    }

    // Move the to the top to hide any message or item box behind
    public void MoveUpwardInHierarchy()
    {
        transform.SetAsLastSibling();
    }

    // Switch interface
    void SwitchInterface(GameObject requestedInterface)
    {
        currentInterface = requestedInterface;

        // Turn off all menus
        storeInterface.SetActive(false);
        collectionInterface.SetActive(false);
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
    public void ChooseStoreCategory()
    {
        // Clean previous category items.
        foreach (Transform item in storeSlots.transform)
        {
            Destroy(item.gameObject);
        }

        // Enable previous category button.
        if (currentStoreCategory != null)
        {
            currentStoreCategory.GetComponent<Button>().interactable = true;
        }

        // Get current category
        currentStoreCategory = EventSystem.current.currentSelectedGameObject;
        string category = currentStoreCategory.name.Split(" ")[0];

        // Set current category to disabled
        // Remain selected sprite state when choosing item under current category
        currentStoreCategory.GetComponent<Button>().interactable = false;

        List<ItemClass> currentItems = new();
        switch (category)
        {
            case "Clothes":
                currentItems = inventory.clothes.Values.ToList<ItemClass>();
                break;
            case "Potion":
                currentItems = inventory.potions.Values.ToList<ItemClass>();
                break;
            default:
                break;
        }

        foreach (ItemClass item in currentItems)
        {
            GameObject itemSlot = Instantiate(storeItemSlot, storeSlots.transform);
            itemSlot.transform.GetChild(0).GetComponent<Image>().sprite = item.itemIcon;
            itemSlot.GetComponent<Button>().onClick.AddListener(ChooseStoreItem);
            itemSlot.name = category + "-" + item.name;
        }
    }

    // Show selected item detail information
    public void ChooseStoreItem()
    {
        string[] objName = EventSystem.current.currentSelectedGameObject.name.Split("-");
        string category = objName[0];
        currentStoreItemName = objName[1];

        ItemClass item = null;
        string introText = "";
        buyButton.SetActive(true);
        buyButton.GetComponentInChildren<TMP_Text>().text = "Buy";

        switch (category)
        {
            case "Clothes":
                item = inventory.clothes[currentStoreItemName];
                introText += "Type: <gradient=GoldWhite>" + item.GetClothes().clothesType + "</gradient>\n";
                introText += "Price: <gradient=GoldWhite>" + item.GetClothes().price + "</gradient>\n";
                if (item.GetClothes().price == 0)
                {
                    buyButton.GetComponentInChildren<TMP_Text>().text = "Free";
                }
                break;
            case "Potion":
                item = inventory.potions[currentStoreItemName];
                introText += "Type: <gradient=GoldWhite>" + item.GetPotion().potionType + "</gradient>\n";
                if (item.GetPotion().potionType == PotionClass.PotionType.LuckyPower)
                {
                    introText += "Gain Lucky: <gradient=GoldWhite>+" + item.GetPotion().lucky * 100 + "%</gradient>\n";
                }
                else
                {
                    introText += "Gain Buff: <gradient=GoldWhite>X" + item.GetPotion().buff + "</gradient>\n";
                }
                introText += "Price: <gradient=GoldWhite>" + item.GetPotion().price + "</gradient>\n";
                break;
            default:
                break;
        }
        // If is in tutorial level, no items canbe bought.
        if (gameController.isTutorialOn == 1) {
            buyButton.SetActive(false);
        }

        storeItemDetailPanel.transform.GetChild(0).GetComponent<Image>().enabled = true;
        storeItemDetailPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().enabled = true;
        storeItemDetailPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = item.itemIcon;
        if (PlayerPrefs.GetInt(currentStoreItemName+ "_new") == 1)
        {
            storeItemDetailPanel.transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>().text = "?";
            storeItemDetailPanel.transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>().alignment = TextAlignmentOptions.Center;
            buyButton.SetActive(false);
        }
        else
        {
            string basicText = "Item Name: <gradient=GoldWhite>" + currentStoreItemName + "</gradient>\n" +
                "Item Intro: <gradient=GoldWhite>" + item.itemIntro + "</gradient>\n";
            storeItemDetailPanel.transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>().text =  basicText + introText;
            storeItemDetailPanel.transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>().alignment = TextAlignmentOptions.Left;
        }
    }

    // Buy item and put in the inventory bar.
    public void BuyItem()
    {
        switch (currentStoreCategory.name.Split(" ")[0])
        {
            case "Clothes":
                if (inventory.clothes[currentStoreItemName].price <= PlayerPrefs.GetInt("coin", 0))
                {
                    SoundManager.instance.PlaySoundCoin();
                    inventory.Add(inventory.clothes[currentStoreItemName], false);
                    playerController.SubCoins(inventory.clothes[currentStoreItemName].price);
                }
                else
                {
                    coinPrompt.enabled = true;
                    coinPrompt.canvasRenderer.SetAlpha(1f);
                    coinPrompt.CrossFadeAlpha(0f, 2f, false);
                }
                break;
            case "Potion":
                if (inventory.potions[currentStoreItemName].price <= PlayerPrefs.GetInt("coin", 0)) 
                {
                    SoundManager.instance.PlaySoundCoin();
                    inventory.Add(inventory.potions[currentStoreItemName], false);
                    playerController.SubCoins(inventory.potions[currentStoreItemName].price);
                }
                else
                {
                    coinPrompt.enabled = true;
                    coinPrompt.canvasRenderer.SetAlpha(1f);
                    coinPrompt.CrossFadeAlpha(0f, 2f, false);
                }
                break;
            default:
                break;
        }
    }



    // ------ Collection interface ------
    public void ShowCollectionInterface()
    {
        SwitchInterface(collectionInterface);
    }

    // Switch collection items category
    public void ChooseCollectionCategory()
    {
        // Clean previous category items.
        foreach (Transform item in collectionSlots.transform)
        {
            Destroy(item.gameObject);
        }

        // Enable previous category button.
        if (currentCollectionCategory != null)
        {
            currentCollectionCategory.GetComponent<Button>().interactable = true;
        }

        // Get current category
        currentCollectionCategory = EventSystem.current.currentSelectedGameObject;
        string category = currentCollectionCategory.name.Split(" ")[0];

        // Set current category to disabled
        // Remain selected sprite state when choosing item under current category
        currentCollectionCategory.GetComponent<Button>().interactable = false;

        List<ItemClass> currentItems = new();
        switch (category)
        {
            case "Tool":
                currentItems = inventory.tools.Values.ToList<ItemClass>();
                break;
            case "Food":
                currentItems = inventory.foods.Values.ToList<ItemClass>();
                break;
            case "Barrier":
                currentItems = inventory.barriers.Values.ToList<ItemClass>();
                break;
            case "Treasure":
                currentItems = inventory.treasures.Values.ToList<ItemClass>();
                break;
            default:
                break;
        }

        foreach (ItemClass item in currentItems)
        {
            GameObject itemSlot = Instantiate(collectionItemSlot, collectionSlots.transform);
            itemSlot.transform.GetChild(0).GetComponent<Image>().sprite = item.itemIcon;
            itemSlot.GetComponent<Button>().onClick.AddListener(ChooseCollectionItem);
            itemSlot.name = category + "-" + item.name;
        }
    }

    // Show selected item detail information
    public void ChooseCollectionItem()
    {
        string[] objName = EventSystem.current.currentSelectedGameObject.name.Split("-");
        string category = objName[0];
        currentCollectionItemName = objName[1];

        RefreshStoreUI(currentCollectionItemName, category);
    }

    // Put item in the inventory bar.
    public void UseItem()
    {
        switch (currentCollectionCategory.name.Split(" ")[0])
        {
            case "Treasure":
                int currentQuantity = PlayerPrefs.GetInt(currentCollectionItemName + "_quantity");
                if (currentQuantity > 0)
                {
                    PlayerPrefs.SetInt(currentCollectionItemName + "_quantity", currentQuantity - 1);
                    inventory.Add(inventory.treasures[currentCollectionItemName], false);
                    RefreshStoreUI(currentCollectionItemName, "Treasure");  
                }
                else
                {   
                    SoundManager.instance.PlaySoundAlert();
                    print("There is no " + currentCollectionItemName + " to use!");
                }
                break;
            default:
                break;
        }
    }

    // Refesh current selected item shows in the detail panel
    public void RefreshStoreUI(string changedItem, string category)
    {
        if (changedItem == currentCollectionItemName)
        {
            ItemClass item = null;
            string introText = "";
            useButton.SetActive(true);
            useButton.GetComponentInChildren<TMP_Text>().text = "Use";

            switch (category)
            {
                case "Tool":
                    item = inventory.tools[currentCollectionItemName];
                    introText += "Type: <gradient=GoldWhite>" + item.GetTool().toolType + "</gradient>\n";
                    introText += "Damage: <gradient=GoldWhite>" + item.GetTool().damage + "</gradient>\n";
                    introText += "Range: <gradient=GoldWhite>" + item.GetTool().range + "</gradient>\n";
                    introText += "Bounty: <gradient=GoldWhite>" + item.GetTool().bounty + "</gradient>\n";
                    useButton.SetActive(false);
                    break;
                case "Treasure":
                    item = inventory.treasures[currentCollectionItemName];
                    introText += "Type: <gradient=GoldWhite>" + item.GetTreasure().treasureType + "</gradient>\n";
                    if (PlayerPrefs.GetInt(currentCollectionItemName + "_quantity") == 0)
                    {
                        introText += "Quantity: <gradient=Red>";
                    }
                    else
                    {
                        introText += "Quantity: <gradient=Green>";
                    }
                    introText += PlayerPrefs.GetInt(currentCollectionItemName + "_quantity") + "</gradient>\n";
                    useButton.SetActive(true);
                    break;
                case "Food":
                    item = inventory.foods[currentCollectionItemName];
                    introText += "Type: <gradient=GoldWhite>" + item.GetFood().foodType + "</gradient>\n";
                    introText += "Gain HP: <gradient=GoldWhite>" + item.GetFood().healthAdded + "</gradient>\n";
                    introText += "Bounty: <gradient=GoldWhite>" + item.GetFood().bounty + "</gradient>\n";
                    useButton.SetActive(false);
                    break;
                case "Barrier":
                    item = inventory.barriers[currentCollectionItemName];
                    introText += "Type: <gradient=GoldWhite>" + item.GetBarrier().barrierType + "</gradient>\n";
                    if (item.GetBarrier().barrierType == BarrierClass.BarrierType.Trader)
                    {
                        introText += "What does it want:";
                        introText += " <gradient=GoldWhite>" + string.Join(' ', item.GetBarrier().getTreasureNameList()) + "</gradient>\n";
                    }
                    else
                    {
                        introText += "HP: <gradient=GoldWhite>" + item.GetBarrier().hp + "</gradient>\n";
                        introText += "How To Destroy:";
                        introText += " <gradient=GoldWhite>" + string.Join(' ', item.GetBarrier().getToolNameList()) + "</gradient>\n";
                        introText += "Drop Treasures: <gradient=GoldWhite>";
                        for (int i = 0; i < item.GetBarrier().dropTreasureProbs.Count; i++)
                        {
                            introText += item.GetBarrier().getDropNameList()[i] + "(" + item.GetBarrier().dropTreasureProbs[i] * 100 + "%) ";
                        }
                        introText += "</gradient>\n";
                    }
                    introText += "Bounty: <gradient=GoldWhite>" + item.GetBarrier().bounty + "</gradient>\n";
                    useButton.SetActive(false);
                    break;
                default:
                    break;
            }
            // If is in tutorial level, no items canbe bought.
            if (gameController.isTutorialOn == 1)
            {
                useButton.SetActive(false);
            }

            collectionItemDetailPanel.transform.GetChild(0).GetComponent<Image>().enabled = true;
            collectionItemDetailPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().enabled = true;
            collectionItemDetailPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = item.itemIcon;
            if (PlayerPrefs.GetInt(currentCollectionItemName + "_new") == 1)
            {
                collectionItemDetailPanel.transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>().text = "?";
                collectionItemDetailPanel.transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>().alignment = TextAlignmentOptions.Center;
                useButton.SetActive(false);
            }
            else
            {
                string basicText = "Item Name: <gradient=GoldWhite>" + currentCollectionItemName + "</gradient>\n" +
                    "Item Intro: <gradient=GoldWhite>" + item.itemIntro + "</gradient>\n";
                collectionItemDetailPanel.transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>().text = basicText + introText;
                collectionItemDetailPanel.transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>().alignment = TextAlignmentOptions.Left;
            }

        }   
    }



    // ------ Pause interface ------
    public void ShowPauseInterface()
    {
        SwitchInterface(pauseInterface);
    }

    // Close current interface and continue playing
    public void Continue()
    {
        SoundManager.instance.PlaySoundButtonClick();
        currentInterface.SetActive(false);
        gameController.EnableWholeScene();
    }

    // Reload current level
    public void Reload()
    {
        StartCoroutine(CReload());
        IEnumerator CReload() {
            SoundManager.instance.PlaySoundButtonClick();
            yield return new WaitForSeconds(0.1f);
            currentInterface.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // Quit current level and go to the start menu scene
    public void Quit() 
    {
        StartCoroutine(CQuit());
        IEnumerator CQuit() {
            SoundManager.instance.PlaySoundButtonClick();
            yield return new WaitForSeconds(0.1f);
            SceneManager.LoadScene("StartMenu");
        }
    }



    // ------ Settings interface ------ 
    public void ShowSettingsInterface()
    {
        SwitchInterface(settingsInterface);
    }

    // toggle sound
    public void SetSound()
    {
        // Disable sound
        if (PlayerPrefs.GetInt("Sound", 1) == 1)
        {
            soundButton.GetComponent<Image>().sprite = soundDisable;
            PlayerPrefs.SetInt("Sound", 0);
            soundManager.DisableAll();

        }
        else // Enable sound
        {
            soundButton.GetComponent<Image>().sprite = soundEnable;
            PlayerPrefs.SetInt("Sound", 1);
            soundManager.EnableAll();
        }
    }

    public void RefreshSettingUI()
    {
        if (PlayerPrefs.GetInt("Sound", 1) == 1)
        {
            soundButton.GetComponent<Image>().sprite = soundEnable;
        }
        else
        {
            soundButton.GetComponent<Image>().sprite = soundDisable;
        }
    }
}
