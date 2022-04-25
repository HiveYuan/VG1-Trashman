using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Trashman {
    public class Health : MonoBehaviour {
        public Image fill;
        public TMP_Text hp;
        public Image prompt;
        public Sprite add_hp;
        public Sprite sub_hp;

        public void SetHealth( float currentHP, float maxHP ) {
            fill.fillAmount = (currentHP / maxHP);
            hp.text = currentHP.ToString("F2")+"/"+maxHP;
        }

        public void SetPrompt(bool isAdd) {
            if (isAdd)
            {
                prompt.sprite = add_hp;
            }
            else
            {
                prompt.sprite = sub_hp;
            }
            prompt.canvasRenderer.SetAlpha(1f);
            prompt.enabled = true;
            prompt.CrossFadeAlpha(0f, 2f, false);
        }
    }
}
