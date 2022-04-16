using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace myGUI
{
    // FIXME: fix the scroll view
    public class HintBox : MonoBehaviour
    {
        public TMP_Text title;

        public TMP_Text content;

        public TMP_Text hint;

        public Image traderBarrier;

        public GameController gameController;

        List<string> hintList = new();

        // Start is called before the first frame update
        void Start()
        {
            gameController = GameObject.Find("GameManager").GetComponent<GameController>();
        }

        public void show(string titleStr, string contentStr, Sprite itemIcon, List<string> nameList)
        {
            title.text = titleStr;
            content.text = contentStr;
            traderBarrier.sprite = itemIcon;
            hintList = nameList;
        }

        public void onClickConfirm()
        {
            string btn = EventSystem.current.currentSelectedGameObject.name;
            Debug.Log(btn);
            gameController.EnableWholeScene();
            Destroy(gameObject);
        }

        public void onClickHint()
        {
            Debug.Log(EventSystem.current.currentSelectedGameObject);
            EventSystem.current.currentSelectedGameObject.SetActive(false);
            hint.text = string.Join(' ', hintList);
        }

        public void Drag()
        {
            Vector3 vec3 = Input.mousePosition;
            Vector3 pos = transform.GetComponent<RectTransform>().position;
            Vector3 off = Input.mousePosition - vec3;
            //vec3 = Input.mousePosition;
            pos = pos + off;
            transform.GetComponent<RectTransform>().position = pos;
        }
    }
}
