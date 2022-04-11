using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class InterfaceManager : MonoBehaviour
{
    // Outlets
    public GameObject storeInterface;
    public GameObject mapInterface;
    public GameObject pauseInterface;
    public GameObject settingsInterface;
    public GameObject gameManager;
    public GameController gameController;

    // Tracking state
    GameObject currentInterface;

    // Methods
    void Awake()
    {
        gameObject.SetActive(false);
        gameController = gameManager.GetComponent<GameController>();
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

    // TODO: Load store interface
    public void ShowStoreInterface()
    {
        SwitchInterface(storeInterface);
    }

    // TODO: Load items cheet sheet interface
    public void ShowMapInterface()
    {
        SwitchInterface(mapInterface);
    }

    public void ShowPauseInterface(){
        SwitchInterface(pauseInterface);
    }

    // TODO: Load settings interface
    public void ShowSettingsInterface()
    {
        SwitchInterface(settingsInterface);
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
}
