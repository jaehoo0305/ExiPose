using UnityEngine;
using TMPro;

public class InteractionUI : MonoBehaviour
{
    public static InteractionUI Instance { get; private set; } //ΩÃ±€≈Ê

    public GameObject panel;          // InteractionPanel ¿¸√º
    public TextMeshProUGUI text;      // æ»≥ª ≈ÿΩ∫∆Æ

    void Awake()
    {
        Instance = this;
        Hide();
    }

    public void Show(string message)
    {
        if (panel != null)
            panel.SetActive(true);

        if (text != null)
            text.text = message;
    }

    public void Hide()
    {
        if (panel != null)
            panel.SetActive(false);
    }
}
