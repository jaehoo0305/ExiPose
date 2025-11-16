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

    [Header("Chase")]
    public float chaseSpeed = 3.5f;
    public float attackRange = 1f;

    [Header("Attack")]
    public float attackInterval = 1.2f;

    [Header("Movement")]
    public float rotationSpeed = 10f;
    public float gravity = -9.81f;

    CharacterController controller;
    EnemyStateMachine fsm;

    int patrolIndex;
    Vector3 velocity;

    public int PatrolIndex { get => patrolIndex; set => patrolIndex = value; }
    public Vector3 Velocity => velocity;

    float animSpeed;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (sensor == null) sensor = GetComponent<EnemySensor>();
        if (animator == null) animator = GetComponentInChildren<Animator>();

        fsm = new EnemyStateMachine();
        fsm.Register(EnemyStateId.Patrol, new EnemyStatePatrol());
        fsm.Register(EnemyStateId.Chase, new EnemyStateChase());
        fsm.Register(EnemyStateId.Attack, new EnemyStateAttack());
    }

    void Start()
    {
        fsm.Initialize(this, EnemyStateId.Patrol);
    }

    void Update()
    {
        float dt = Time.deltaTime;
        fsm.Tick(this, dt);
        ApplyMovement(dt);
        UpdateRotation(dt);  // ← 새로 추가
        UpdateAnimation();
    }

    public void SetVelocity(Vector3 v)
    {
        velocity = v;
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
        if (dir.sqrMagnitude < 0.0001f)
            return;

        RotateTowards(dir, dt);
    }

    public void MoveSeek(Vector3 targetPos, float maxSpeed)
    {
        Vector3 toTarget = targetPos - transform.position;
        toTarget.y = 0f;

        if (toTarget.sqrMagnitude < 0.0001f)
        {
            SetVelocity(Vector3.zero);
            return;
        }

        Vector3 desired = toTarget.normalized * maxSpeed;
        SetVelocity(desired);
    }

    public void MoveArrive(Vector3 targetPos, float maxSpeed, float slowRadius)
    {
        Vector3 toTarget = targetPos - transform.position;
        toTarget.y = 0f;
        float dist = toTarget.magnitude;
        if (dist < 0.01f)
        {
            SetVelocity(Vector3.zero);
            return;
        }

        float speed = maxSpeed;
        if (dist < slowRadius)
            speed = maxSpeed * (dist / slowRadius);

        Vector3 desired = toTarget.normalized * speed;
        SetVelocity(desired);
    }

    public void ChangeState(EnemyStateId id)
    {
        fsm.ChangeState(this, id);
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

        // 부드럽게 보간 (숫자 10은 반응 속도, 5~12 사이에서 취향껏)
        animSpeed = Mathf.Lerp(animSpeed, rawSpeed, Time.deltaTime * 10f);

        animator.SetFloat("Speed", animSpeed);
    }

    public bool IsTargetInAttackRange()
    {
        if (sensor == null || sensor.target == null) return false;
        Vector3 toTarget = sensor.target.position - transform.position;
        toTarget.y = 0f;
        return toTarget.magnitude <= attackRange;
    }
}
