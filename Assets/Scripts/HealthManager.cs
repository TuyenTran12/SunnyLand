using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public float healthAmount = 100f;

    void Update()
    {

    }

    public void takeDamage(float damage)
    {
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / 100f;
    }
    public void heal(float healingAmount)
    {
        healingAmount += healingAmount;
        healingAmount = Mathf.Clamp(healthAmount, 0, 100);

        healthBar.fillAmount = healingAmount / 100f;
    }
}
