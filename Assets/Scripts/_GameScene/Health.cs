using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class HealthChangedEvent : UnityEvent<int, int> { } // current, max
[System.Serializable]
public class DamagedEvent : UnityEvent<int> { }             // damage amount
[System.Serializable]
public class DiedEvent : UnityEvent { }

public class Health : MonoBehaviour, IDamageable
{
    [Header("Health")]
    public int maxHP = 100;
    public int currentHP = 100;

    [Header("Invulnerability (I-Frames)")]
    public float invulnDuration = 0.3f; // seconds
    public bool useInvuln = true;

    [Header("Events")]
    public HealthChangedEvent onHealthChanged;
    public DamagedEvent onDamaged;
    public DiedEvent onDied;

    bool isDead;
    float invulnEndTime;

    void Awake()
    {
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        onHealthChanged?.Invoke(currentHP, maxHP);
    }

    public void TakeDamage(int amount, Vector3 hitPoint, Vector3 hitDir)
    {
        if (isDead) return;
        if (amount <= 0) return;

        if (useInvuln && Time.time < invulnEndTime)
            return;

        currentHP -= amount;
        if (currentHP < 0) currentHP = 0;

        if (useInvuln)
            invulnEndTime = Time.time + invulnDuration;

        onDamaged?.Invoke(amount);
        onHealthChanged?.Invoke(currentHP, maxHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (isDead) return;
        if (amount <= 0) return;

        currentHP += amount;
        if (currentHP > maxHP) currentHP = maxHP;

        onHealthChanged?.Invoke(currentHP, maxHP);
    }

    void RaiseHealthChanged()
    {
        onHealthChanged?.Invoke(currentHP, maxHP);
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        onDied?.Invoke();
    }
}
