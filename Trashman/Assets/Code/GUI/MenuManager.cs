using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject levelsMenu;
    public GameObject settingsMenu;

    // Start is called before the first frame update
    void Start()
    {
        ShowMainMenu();
    }

    // More organized version
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
        SwitchMenu(levelsMenu);
    }

    // Load tutorial/current level.
    public void StartPlay() {
        SceneManager.LoadScene("Tutorial");  // TODO: Load current level (need to store level info: Playerprefs) scene
    }

    // TODO: quit?
    public void QuitGame() {

    }

    // TODO: change to panel view; consider use prefab and list to organize 
    // load requested level scene
    public void LoadLevel()
    {
        string levelName = EventSystem.current.currentSelectedGameObject.name;
        char level = levelName[levelName.Length - 1];
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
