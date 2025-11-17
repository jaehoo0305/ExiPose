using UnityEngine;

public class EnemySensor : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("View")]
    public float viewDistance = 10f;
    public float viewHalfAngle = 45f;
    public LayerMask obstacleMask;

    [Header("Debug")]
    public bool drawGizmos = true;

    public bool CanSeeTarget { get; private set; }
    public float lastSeenTime { get; private set; }

    float cosHalfAngle;

    void Awake()
    {
        RecalculateCosHalfAngle();
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        RecalculateCosHalfAngle();
    }
#endif

    void RecalculateCosHalfAngle()
    {
        cosHalfAngle = Mathf.Cos(viewHalfAngle * Mathf.Deg2Rad);
    }

    void LateUpdate()
    {
        UpdateSensor();
    }

    void UpdateSensor()
    {
        if (target == null)
        {
            CanSeeTarget = false;
            return;
        }

        Vector3 toTarget = target.position - transform.position;
        float dist = toTarget.magnitude;

        if (dist > viewDistance)
        {
            CanSeeTarget = false;
            return;
        }

        toTarget.y = 0f;
        Vector3 fwd = transform.forward;
        float dot = Vector3.Dot(fwd.normalized, toTarget.normalized);

        if (dot < cosHalfAngle)
        {
            CanSeeTarget = false;
            return;
        }

        Vector3 origin = transform.position + Vector3.up * 1.6f;

        if (Physics.Raycast(origin, toTarget.normalized, out RaycastHit hit, viewDistance, ~obstacleMask))
        {
            if (hit.transform == target)
            {
                CanSeeTarget = true;
                lastSeenTime = Time.time;
                return;
            }
        }

        CanSeeTarget = false;
    }

    public bool HasRecentlySeenTarget(float memoryDuration)
    {
        return Time.time - lastSeenTime <= memoryDuration;
    }

    void OnDrawGizmosSelected()
    {
        if (!drawGizmos) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 forward = transform.forward;
        Quaternion leftRot = Quaternion.AngleAxis(-viewHalfAngle, Vector3.up);
        Quaternion rightRot = Quaternion.AngleAxis(viewHalfAngle, Vector3.up);

        Vector3 leftDir = leftRot * forward;
        Vector3 rightDir = rightRot * forward;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + leftDir * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + rightDir * viewDistance);
    }
}
