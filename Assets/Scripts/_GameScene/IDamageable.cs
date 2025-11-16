using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int amount, Vector3 hitPoint, Vector3 hitDirection);
}
