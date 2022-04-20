using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Trashman;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject levelsMenu;
    public GameObject settingsMenu;

    // TODO: more level groups
    [SerializeField] private GameObject levelGroups;
    private List<Button> levels = new();

    // Start is called before the first frame update
    void Start()
    {
        ShowMainMenu();

        // set all the levels
        int levelGroupNums = levelGroups.transform.childCount;
        for (int i = 0; i < levelGroupNums; i++)
        {
            GameObject levelGroup = levelGroups.transform.GetChild(i).gameObject;
            //levels = new Button[levelGroup.transform.childCount];
            for (int j = 0; j < levelGroup.transform.childCount; j++)
            {
                Button level = levelGroup.transform.GetChild(j).GetComponent<Button>();
                level.onClick.AddListener(LoadLevel);
                levels.Add(level);
            }
        }
    }

    // More organized version
    public void RefreshUI()
    {
        int currentLevel = PlayerPrefs.GetInt("Level", 0);
        for (int i = 0; i < levels.Count; i++)
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
        StartCoroutine(CStartPlay());
        IEnumerator CStartPlay() {
            SoundManager.instance.PlaySoundButtonClick();
            int level = PlayerPrefs.GetInt("Level", 0);
            yield return new WaitForSeconds(0.1f);
            SceneManager.LoadScene("Level-" + level);
        }
    }

    // TODO: current function set to clear the level and first-time prefs to test functionality
    public void QuitGame() {
        PlayerPrefs.DeleteAll();
    }

    public void LoadLevel()
    {
        string levelString = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TMP_Text>().text;
        if (levelString == "")
        {
            // TODO: play wrong/unclickable sound or msg box to inform player
            Debug.Log("Locked level!");
        }
        else
        {
            StartCoroutine(CLoad());
            IEnumerator CLoad() {
                SoundManager.instance.PlaySoundButtonClick();
                int level = int.Parse(levelString);
                yield return new WaitForSeconds(0.1f);
                SceneManager.LoadScene("Level-" + level);
            }
        }
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
