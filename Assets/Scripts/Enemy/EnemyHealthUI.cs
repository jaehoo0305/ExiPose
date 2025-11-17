using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public Health health;
    public Slider slider;

    void Start()
    {
        if (health != null)
        {
            health.onHealthChanged.AddListener(UpdateUI);
            UpdateUI(health.currentHP, health.maxHP);
        }
    }

    void UpdateUI(int cur, int max)
    {
        slider.maxValue = max;
        slider.value = cur;
    }
}
