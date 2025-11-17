using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyBase : MonoBehaviour
{
    [Header("Refs")]
    public EnemySensor sensor;
    public Animator animator;

    [Header("Patrol")]
    public Transform[] patrolPoints;
    public float patrolSpeed = 2f;

    [Header("Chase / Attack")]
    public float chaseSpeed = 3.5f;
    public float attackEnterRange = 1.2f;   // start attacking at this distance
    public float attackKeepRange = 1.8f;   // keep attacking while within this distance
    public float attackInterval = 1.0f;   // seconds between attacks

    [Header("Movement")]
    public float rotationSpeed = 10f;
    public float gravity = -9.81f;

    CharacterController controller;
    EnemyStateMachine stateMachine;

    int patrolIndex;
    Vector3 velocity;
    float animSpeed;

    public int PatrolIndex
    {
        get => patrolIndex;
        set => patrolIndex = value;
    }

    public Vector3 Velocity => velocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (sensor == null) sensor = GetComponent<EnemySensor>();
        if (animator == null) animator = GetComponentInChildren<Animator>();

        stateMachine = new EnemyStateMachine();
        stateMachine.Register(EnemyStateId.Patrol, new EnemyStatePatrol());
        stateMachine.Register(EnemyStateId.Chase, new EnemyStateChase());
        stateMachine.Register(EnemyStateId.Attack, new EnemyStateAttack());
    }

    void Start()
    {
        stateMachine.Initialize(this, EnemyStateId.Patrol);
    }

    void Update()
    {
        float dt = Time.deltaTime;
        stateMachine.Tick(this, dt);

        ApplyMovement(dt);
        UpdateRotation(dt);
        UpdateAnimation();
    }

    public void SetVelocity(Vector3 v)
    {
        velocity = v;
    }

    public void MoveSeek(Vector3 targetPos, float speed)
    {
        Vector3 toTarget = targetPos - transform.position;
        toTarget.y = 0f;

        if (toTarget.sqrMagnitude < 0.0001f)
        {
            velocity = Vector3.zero;
            return;
        }

        Vector3 desired = toTarget.normalized * speed;
        velocity = desired;
    }

    public void MoveArrive(Vector3 targetPos, float speed, float slowRadius)
    {
        Vector3 toTarget = targetPos - transform.position;
        toTarget.y = 0f;
        float dist = toTarget.magnitude;

        if (dist < 0.01f)
        {
            velocity = Vector3.zero;
            return;
        }

        float finalSpeed = speed;
        if (dist < slowRadius)
            finalSpeed = speed * (dist / slowRadius);

        Vector3 desired = toTarget.normalized * finalSpeed;
        velocity = desired;
    }

    public void RotateTowards(Vector3 dir, float dt)
    {
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f)
            return;

        dir.Normalize();
        Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * dt);
    }

    void UpdateRotation(float dt)
    {
        Vector3 dir = new Vector3(velocity.x, 0f, velocity.z);
        if (dir.sqrMagnitude < 0.0001f) return;
        RotateTowards(dir, dt);
    }

    void ApplyMovement(float dt)
    {
        Vector3 move = velocity;
        move.y += gravity * dt;
        controller.Move(move * dt);
    }

    void UpdateAnimation()
    {
        if (animator == null) return;
        float rawSpeed = new Vector3(velocity.x, 0f, velocity.z).magnitude;
        animSpeed = Mathf.Lerp(animSpeed, rawSpeed, Time.deltaTime * 10f);
        animator.SetFloat("Speed", animSpeed);
    }

    public Transform GetCurrentPatrolPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return null;
        return patrolPoints[patrolIndex];
    }

    public void NextPatrolPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;
        patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
    }

    public void ChangeState(EnemyStateId id)
    {
        stateMachine.ChangeState(this, id);
    }

    public bool IsTargetWithinEnterRange()
    {
        if (sensor == null || sensor.target == null) return false;

        Vector3 toTarget = sensor.target.position - transform.position;
        toTarget.y = 0f;
        float dist = toTarget.magnitude;

        return dist <= attackEnterRange;
    }

    public bool IsTargetInAttackRange()
    {
        if (sensor == null || sensor.target == null) return false;

        Vector3 toTarget = sensor.target.position - transform.position;
        toTarget.y = 0f;
        float dist = toTarget.magnitude;

        // use keep range here
        return dist <= attackKeepRange;
    }
}
