using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class InterfaceManager : MonoBehaviour
{
    // Outlets
    public GameObject storeInterface;
    public GameObject itemsCheetSheetInterface;
    public GameObject pauseInterface;
    public GameObject settingsInterface;

    // TODO: disable scene
    // Tracking state
    public GameObject currentInterface;

    // Switch interface
    void SwitchInterface(GameObject requestedInterface)
    {
        currentInterface = requestedInterface;

        // Turn off all menus
        storeInterface.SetActive(false);
        itemsCheetSheetInterface.SetActive(false);
        pauseInterface.SetActive(false);
        settingsInterface.SetActive(false);

        // Turn on requested menu
        requestedInterface.SetActive(true);
    }

    // TODO: Load store interface
    public void ShowStoreInterface()
    {
        SwitchInterface(storeInterface);
    }

    // TODO: Load items cheet sheet interface
    public void ShowItemsInterface()
    {
        SwitchInterface(itemsCheetSheetInterface);
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
    }
}
