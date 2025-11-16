using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public Health health;
    public Slider slider;
    public TextMeshProUGUI txt;

    void Start()
    {
        health.onHealthChanged.AddListener(UpdateUI);
        UpdateUI(health.currentHP, health.maxHP);
    }

    void UpdateUI(int cur, int max)
    {
        slider.maxValue = max;
        slider.value = cur;
        txt.text = $"{cur} / {max}";
    }
}
