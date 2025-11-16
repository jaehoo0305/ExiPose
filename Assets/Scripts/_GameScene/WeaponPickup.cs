using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private string weaponName = "Plunger";

    public void OnPickUp()
    {
        WeaponEquipManager equipManager = FindObjectOfType<WeaponEquipManager>();
        if (equipManager == null)
        {
            Debug.LogWarning("[WeaponPickup] WeaponEquipManager not found in scene.");
            return;
        }

        equipManager.Equip(weaponName);

        // Hide interaction UI
        if (InteractionUI.Instance != null)
        {
            InteractionUI.Instance.HideNow();
        }

        // Disable pickup object
        gameObject.SetActive(false);
    }
}
