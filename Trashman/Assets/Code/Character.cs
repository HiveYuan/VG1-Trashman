using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float maxHealth = 40f;
    public float currentHealth;

    public Health health;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            GainHealth(2f);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            LoseHealth(2f);
        }
    }

    void GainHealth(float hp) {
        if (currentHealth < maxHealth) {
            currentHealth += hp;
        }

        health.SetHealth(currentHealth, maxHealth);
    }

    void LoseHealth(float hp)
    {
        if (currentHealth > 0f)
        {
            currentHealth -= hp;
        }

        health.SetHealth(currentHealth, maxHealth);
    }
}
