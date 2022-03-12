using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Trashman {
    public class Character : MonoBehaviour {
        public float maxHealth = 40f;
        public float currentHealth;

        public Health health;
        public Image prompt;
        public InventoryManager inventory;
        public Image introBox;

        // Start is called before the first frame update
        void Start() {
            currentHealth = maxHealth;
            prompt.enabled = false;
            introBox.enabled = false;
            introBox.transform.GetChild(0).GetComponent<Image>().enabled = false;
            introBox.transform.GetChild(0).GetComponent<Image>().transform.GetChild(0).GetComponent<Image>().enabled = false;
        }

        // Update is called once per frame
        void Update() {
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                GainHealth(2f);
            } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
                LoseHealth(2f);
            }

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                introBox.enabled = true;
                introBox.transform.GetChild(0).GetComponent<Image>().enabled = true;
                introBox.transform.GetChild(0).GetComponent<Image>().transform.GetChild(0).GetComponent<Image>().enabled = true;
                introBox.transform.GetChild(0).GetComponent<Image>().transform.GetChild(0).GetComponent<Image>().sprite = inventory.foods["WholeBurger"].itemIcon;
                introBox.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = inventory.foods["WholeBurger"].itemName;
                introBox.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = inventory.foods["WholeBurger"].itemIntro;
            }
        }

        void GainHealth( float hp ) {
            if (currentHealth < maxHealth) {
                currentHealth += hp;
                health.SetHealth(currentHealth, maxHealth);
                health.SetPrompt(true);
            }
        }

        void LoseHealth( float hp ) {
            if (currentHealth > 0f) {
                currentHealth -= hp;
                health.SetHealth(currentHealth, maxHealth);
                health.SetPrompt(false);
            }
        }
    }
}