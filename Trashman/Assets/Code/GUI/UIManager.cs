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
    public GameObject gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            
            CreateMsgBox("test1","test2");
        }
    }

    public MessageBox CreateMsgBox(string title, string content)
    {
        GameObject box = Instantiate(msgBox, GameObject.Find("Canvas").transform);
        MessageBox mbox = box.GetComponent<MessageBox>();
        mbox.show(title, content);
        return mbox;
    }

    public ItemBox CreateItemBox(string title, string content, Sprite icon)
    {
        GameObject box = Instantiate(itemBox, GameObject.Find("Canvas").transform);
        ItemBox ibox = box.GetComponent<ItemBox>();
        ibox.show(title, content, icon);
        return ibox;
    }
}
