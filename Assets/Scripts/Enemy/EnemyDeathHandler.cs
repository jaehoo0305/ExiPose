using UnityEngine;

public class EnemyDeathHandler : MonoBehaviour
{
    public EnemyBase enemyBase;
    public Animator animator;
    public GameObject hpCanvas;
    public float destroyDelay = 5f;

    public Collider[] extraColliders;
    public MonoBehaviour[] extraScriptsToDisable;

    public CapsuleCollider deathCollider;
    public Rigidbody rb;

    public float dropOffset = 0.4f;

    bool isHandled;

    void Reset()
    {
        if (enemyBase == null)
            enemyBase = GetComponent<EnemyBase>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    public void OnDeath()
    {
        if (isHandled) return;
        isHandled = true;

        // stop AI
        if (enemyBase != null)
            enemyBase.enabled = false;

        // disable CharacterController
        var cc = GetComponent<CharacterController>();
        if (cc != null)
            cc.enabled = false;

        // disable extra colliders
        if (extraColliders != null)
        {
            for (int i = 0; i < extraColliders.Length; i++)
            {
                if (extraColliders[i] != null)
                    extraColliders[i].enabled = false;
            }
        }

        // disable extra scripts (sensor, attack hitbox, etc.)
        if (extraScriptsToDisable != null)
        {
            for (int i = 0; i < extraScriptsToDisable.Length; i++)
            {
                if (extraScriptsToDisable[i] != null)
                    extraScriptsToDisable[i].enabled = false;
            }
        }

        // hide HP UI
        if (hpCanvas != null)
            hpCanvas.SetActive(false);

        // enable death collider + physics
        if (deathCollider != null)
            deathCollider.enabled = true;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        // move slightly down so body touches the ground
        transform.position += Vector3.down * dropOffset;

        // play death animation
        if (animator != null)
            animator.SetTrigger("Die");

        // destroy after delay
        if (destroyDelay > 0f)
            Destroy(gameObject, destroyDelay);
    }
}
