using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SubMenuManager : MonoBehaviour
{
    public GameObject storeInterface;
    public GameObject itemsCheetSheetInterface;
    public GameObject pauseInterface;
    public GameObject settingsInterface;


    // Switch interface
    void SwitchInterface(GameObject requestedInterface)
    {
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

    // TODO: Load pause game interface
    public void ShowPauseInterface(){
        SwitchInterface(pauseInterface);
    }

    // TODO: Load settings interface
    public void ShowSettingsInterface()
    {
        SwitchInterface(settingsInterface);
    }
}
