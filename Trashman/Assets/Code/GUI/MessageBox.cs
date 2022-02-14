using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace myGUI{
    public class MessageBox : MonoBehaviour
    {
        public Text title;

        public Text content;

        public Button confirm;

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("test start");
            show("test show", "this is to test show func");
            //gameObject.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void show(string titleStr, string contentStr)
        {
            title.text = titleStr;
            content.text = contentStr;
        }

        public void onClickConfirm()
        {
            Debug.Log("confirm test");
            Destroy(gameObject);
        }
        
        public void Drag()
        {
            Vector3 vec3 = Input.mousePosition;
            Vector3 pos = transform.GetComponent<RectTransform>().position;
            Vector3 off = Input.mousePosition - vec3;
            vec3 = Input.mousePosition;
            pos = pos + off;
            transform.GetComponent<RectTransform>().position = pos;
        }
    }
}
