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

        public Image barrier;

        public GameController gameController;

        List<string> hintRequiredList = new();
        List<string> hintDropList = new();

        // Start is called before the first frame update
        void Start()
        {
            gameController = GameObject.Find("GameManager").GetComponent<GameController>();
        }

        public void show(string titleStr, string contentStr, Sprite itemIcon, List<string> requiredList, List<string> dropList)
        {
            title.text = titleStr;
            content.text = contentStr;
            barrier.sprite = itemIcon;
            hintRequiredList = requiredList;
            hintDropList = dropList;
        }

        public void onClickConfirm()
        {
            string btn = EventSystem.current.currentSelectedGameObject.name;
            gameController.EnableWholeScene();
            Destroy(gameObject);
        }

        public void onClickHint()
        {
            Debug.Log(EventSystem.current.currentSelectedGameObject);
            EventSystem.current.currentSelectedGameObject.SetActive(false);
            if (hintDropList.Count == 0)
            {
                hint.text = "Trade with: ";
            }
            else
            {
                hint.text = "Attack with: ";
            }
            hint.text += string.Join(' ', hintRequiredList) + "\n";
            hint.text += "May drop: ";
            hint.text += string.Join(' ', hintDropList);
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
