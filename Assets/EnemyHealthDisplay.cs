using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthDisplay : MonoBehaviour
{
    public Slider healthBar;
    public Vector3 positionOffset;
    public void UpdateHealthBar(float health, float maxHealth)
    {
        healthBar.value = health/maxHealth;
    }

    public void Update()
    {
        healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + positionOffset);
    }
}
