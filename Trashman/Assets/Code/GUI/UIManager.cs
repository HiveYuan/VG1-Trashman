using System.Collections;
using System.Collections.Generic;
using myGUI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject msgBox;
    public GameObject itemBox;
    public GameObject hintBox;
    public GameObject gameManager;
    public GameController gameController;
    
    // Start is called before the first frame update
    void Start()
    {
        gameController = gameManager.GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public MessageBox CreateMsgBox(string title, string content)
    {
        GameObject box = Instantiate(msgBox, GameObject.Find("Canvas").transform);
        MessageBox mbox = box.GetComponent<MessageBox>();
        mbox.show(title, content);
        gameController.DisableWholeScene();
        return mbox;
    }

    public ItemBox CreateItemBox(string title, string content, Sprite icon)
    {
        GameObject box = Instantiate(itemBox, GameObject.Find("Canvas").transform);
        ItemBox ibox = box.GetComponent<ItemBox>();
        ibox.show(title, content, icon);
        gameController.DisableWholeScene();
        return ibox;
    }

    public HintBox CreateHintBox(string title, string content, Sprite icon, List<string> requiredList, List<string> dropList)
    {
        GameObject box = Instantiate(hintBox, GameObject.Find("Canvas").transform);
        HintBox hbox = box.GetComponent<HintBox>();
        hbox.show(title, content, icon, requiredList, dropList);
        gameController.DisableWholeScene();
        return hbox;
    }
}
