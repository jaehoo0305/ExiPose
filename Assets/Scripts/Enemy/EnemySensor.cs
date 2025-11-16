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
    public Vector3 LastSeenPosition { get; private set; }

    float cosHalfAngle;

    void Awake()
    {
        cosHalfAngle = Mathf.Cos(viewHalfAngle * Mathf.Deg2Rad);
    }

    void LateUpdate()
    {
        UpdateSensor();
    }

    void UpdateSensor()
    {
        CanSeeTarget = false;
        if (target == null) return;

        Vector3 toTarget = target.position - transform.position;
        float sqrDist = toTarget.sqrMagnitude;
        if (sqrDist > viewDistance * viewDistance)
            return;

        Vector3 dir = toTarget.normalized;
        float dot = Vector3.Dot(transform.forward, dir);
        if (dot < cosHalfAngle)
            return;

        if (Physics.Raycast(transform.position + Vector3.up * 1.7f, dir, out RaycastHit hit, viewDistance, ~0))
        {
            if (hit.transform == target)
            {
                CanSeeTarget = true;
                LastSeenPosition = target.position;
            }
        }
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
