using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject levelsMenu;
    public GameObject settingsMenu;

    // TODO: more level groups
    [SerializeField] private GameObject levelGroup;
    private Button[] levels;

    // Start is called before the first frame update
    void Start()
    {
        ShowMainMenu();

        // set all the levels
        levels = new Button[levelGroup.transform.childCount];
        for (int i = 0; i < levelGroup.transform.childCount; i++)
        {
            levels[i] = levelGroup.transform.GetChild(i).GetComponent<Button>();
            levels[i].onClick.AddListener(LoadLevel);
        }

    }

    // More organized version
    public void RefreshUI()
    {
        int currentLevel = PlayerPrefs.GetInt("Level", 0);
        for (int i = 0; i < levels.Length; i++)
        {
            if (currentLevel >= i)
            {
                levels[i].transform.GetChild(0).GetComponent<TMP_Text>().text = i + "";
                levels[i].transform.GetChild(1).GetComponent<Image>().enabled = false;
            }
            else
            {
                levels[i].transform.GetChild(0).GetComponent<TMP_Text>().text = "";
                levels[i].transform.GetChild(1).GetComponent<Image>().enabled = true;
            }
            //levels[i].transform.GetChild(1).GetComponent<Image>().sprite = items[i].GetItem().itemIcon;
        }
    }

    void SwitchMenu(GameObject menu) {
        // Turn off all menus
        mainMenu.SetActive(false);
        levelsMenu.SetActive(false);
        settingsMenu.SetActive(false);

        // Turn on requested menu
        menu.SetActive(true);
    }

    // Load main menu.
    public void ShowMainMenu()
    {
        SwitchMenu(mainMenu);
    }

    // Load settings menu.
    public void ShowSettingsMenu()
    {
        SwitchMenu(settingsMenu);
    }

    // Load levels menu.
    public void ShowLevelsMenu()
    {
        RefreshUI();
        SwitchMenu(levelsMenu);
    }

    // Load current level.
    public void StartPlay() {
        int level = PlayerPrefs.GetInt("Level", 0);
        SceneManager.LoadScene("Level-" + level);
    }

    // TODO: quit?
    public void QuitGame() {

    }

    public void LoadLevel()
    {
        int level = int.Parse(EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TMP_Text>().text);
        SceneManager.LoadScene("Level-" + level);
    }

    // TODO: Set sound preference.
    public void SetSound() {

    }

    // TODO: Set display preference.
    public void SetDisplay() {

    }

    // TODO: Set keyboard preference.
    public void SetKeyboard()
    {

    }

}
