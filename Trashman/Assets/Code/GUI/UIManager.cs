using System.Collections;
using System.Collections.Generic;
using myGUI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject msgBox;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject box = Instantiate(msgBox, GameObject.Find("Canvas").transform);
        MessageBox mbox = box.GetComponent<MessageBox>();
        mbox.show("test!show", "this is a test show");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            
            GameObject box = Instantiate(msgBox, GameObject.Find("Canvas").transform);
        }
    }
}
