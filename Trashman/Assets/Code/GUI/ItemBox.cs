using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Trashman;

namespace myGUI{
    public class ItemBox : MonoBehaviour
    {
        public TMP_Text title;

        public TMP_Text content;

        public Image item;

        public Button confirm;

        public GameController gameController;
        
        // Start is called before the first frame update
        void Start()
        {
            gameController = GameObject.Find("GameManager").GetComponent<GameController>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void show(string titleStr, string contentStr, Sprite itemIcon)
        {
            title.text = titleStr;
            content.text = contentStr;
            item.sprite = itemIcon;
        }

        public void onClickConfirm()
        {
            string btn = EventSystem.current.currentSelectedGameObject.name;
            SoundManager.instance.PlaySoundButtonClick();
            gameController.EnableWholeScene();
            Destroy(gameObject);
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
