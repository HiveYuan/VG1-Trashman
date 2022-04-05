using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;

    // Start is called before the first frame update
    void Start()
    {
        settingsMenu = GameObject.Find("Settings Menu");
        settingsMenu.SetActive(false);
    }


    // Main Menu:
    // Load tutorial level.
    public void StartPlay() {
        SceneManager.LoadScene(1); 
    }

    // TODO: add level panel
    void ChooseLevel() {
        
    }

    // Load settings menu.
    public void LoadSettingsMenu() {
        mainMenu = GameObject.Find("Main Menu");
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    // TODO: quit?
    void QuitGame() {

    }

    // Settings Menu:
    // Back to main menu.
    public void BackToMainMenu() {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    // Set sound preference.
    void SetSound() {

    }

    // Set display preference.
    void SetDisplay() {

    }

    // Set keyboard preference.
    void SetKeyboard()
    {

    }

}
