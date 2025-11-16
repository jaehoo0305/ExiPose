using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [Header("Damage")]
    public int damage = 10;
    public LayerMask targetLayers;

    [Header("Hitbox Active")]
    public bool startDisabled = true;

    private Collider hitboxCollider;

    void Awake()
    {
        hitboxCollider = GetComponent<Collider>();
        if (hitboxCollider == null)
        {
            Debug.LogError("[AttackHitbox] Collider is missing.");
            return;
        }

        hitboxCollider.isTrigger = true;

        if (startDisabled)
            hitboxCollider.enabled = false;
    }

    public void EnableHitbox()
    {
        if (hitboxCollider != null)
            hitboxCollider.enabled = true;
    }

    public void DisableHitbox()
    {
        if (hitboxCollider != null)
            hitboxCollider.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        // Layer filter
        if ((targetLayers.value & (1 << other.gameObject.layer)) == 0)
            return;

        IDamageable damageable = other.GetComponentInParent<IDamageable>();
        if (damageable == null) return;

        Vector3 hitPoint = other.ClosestPoint(transform.position);
        Vector3 hitDir = (other.transform.position - transform.position).normalized;

        damageable.TakeDamage(damage, hitPoint, hitDir);
    }
}
