using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEquipManager : MonoBehaviour
{
    [Serializable]
    public class WeaponEntry
    {
        public string weaponName;
        public GameObject prefab; // This must be Plunger_Weapon
    }

    [SerializeField] private Transform handTransform;
    [SerializeField] private List<WeaponEntry> weaponEntries = new List<WeaponEntry>();

    private GameObject currentWeaponInstance;

    public void Equip(string weaponName)
    {
        WeaponEntry entry = weaponEntries.Find(x => x.weaponName == weaponName);
        if (entry == null || entry.prefab == null)
        {
            Debug.LogWarning($"[WeaponEquipManager] Weapon not found: {weaponName}");
            return;
        }

        if (currentWeaponInstance != null)
        {
            Destroy(currentWeaponInstance);
            currentWeaponInstance = null;
        }

        // Instantiate Plunger_Weapon under the hand
        currentWeaponInstance = Instantiate(entry.prefab, handTransform);
        currentWeaponInstance.transform.localScale = Vector3.one;

        AlignByGrip(currentWeaponInstance.transform);
    }

    private void AlignByGrip(Transform weaponRoot)
    {
        Transform grip = weaponRoot.Find("Grip");
        if (grip == null)
        {
            Debug.LogWarning("[WeaponEquipManager] Grip transform not found.");
            return;
        }

        // Make Grip end up at the hand origin (0,0,0, identity) in local space
        Quaternion invRot = Quaternion.Inverse(grip.localRotation);
        weaponRoot.localRotation = invRot;
        weaponRoot.localPosition = -(invRot * grip.localPosition);
    }
}
