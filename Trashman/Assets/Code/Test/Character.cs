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

        // Start is called before the first frame update
        void Start() {
            currentHealth = maxHealth;
            prompt.enabled = false;
        }

        // Update is called once per frame
        void Update() {
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                GainHealth(2f);
            } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
                LoseHealth(2f);
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