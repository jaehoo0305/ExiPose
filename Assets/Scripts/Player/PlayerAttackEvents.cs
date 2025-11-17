using UnityEngine;

public class PlayerAttackEvents : MonoBehaviour
{
    public AttackHitbox hitbox;

    public void EnableHitbox()
    {
        if (hitbox) hitbox.EnableHitbox();
    }

    public void DisableHitbox()
    {
        if (hitbox) hitbox.DisableHitbox();
    }
}
