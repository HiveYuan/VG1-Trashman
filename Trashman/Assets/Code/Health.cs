using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Image fill;

    public void SetHealth(float currentHP, float maxHP) {
        fill.fillAmount = (currentHP / maxHP);
    } 
}
