using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI number;

    public void displayHealth(int health)
    {
        slider.value = health;
        number.text = health.ToString();
    }
}
