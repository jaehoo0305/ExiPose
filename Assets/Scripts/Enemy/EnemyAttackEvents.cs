using UnityEngine;

public class EnemyAttackEvents : MonoBehaviour
{
    public AttackHitbox hitbox;

    public void EnableHitbox() => hitbox.EnableHitbox();
    public void DisableHitbox() => hitbox.DisableHitbox();
}
