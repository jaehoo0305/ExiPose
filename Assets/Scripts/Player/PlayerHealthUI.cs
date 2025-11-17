using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public Health player;
    public Slider hpBar;

    void Start()
    {
        if (player != null)
        {
            player.onHealthChanged.AddListener(UpdateUI);
            UpdateUI(player.currentHP, player.maxHP);
        }
    }

    void UpdateUI(int current, int max)
    {
        hpBar.maxValue = max;
        hpBar.value = current;
    }
}
