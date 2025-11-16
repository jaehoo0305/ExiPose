using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    public Health player;
    public Slider hpBar;
    public TextMeshProUGUI hpText;

    void Start()
    {
        player.onHealthChanged.AddListener(UpdateUI);
        UpdateUI(player.currentHP, player.maxHP);
    }

    void UpdateUI(int current, int max)
    {
        hpBar.maxValue = max;
        hpBar.value = current;
        hpText.text = $"{current} / {max}";
    }
}
