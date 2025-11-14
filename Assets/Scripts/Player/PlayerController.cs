using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public float gravity = -9.81f;
    public float rotateSpeed = 10f;       // 평소 이동 방향 회전 속도

    [Header("Attack")]
    public float attackCooldown = 0.35f; // 공격 연타 방지 쿨타임
    public float attackLockTime = 0.35f; // 공격 중으로 보는 시간(몸 방향 고정)
    public float attackMoveSlowFactor = 0.2f; // 공격 중 이동 속도 배율 (0.2 = 20%)

    private CharacterController controller;
    private Animator anim;
    private Vector3 velocity;

    private float attackTimer = 0f;       // 다음 공격까지 남은 시간
    private float attackLockTimer = 0f;   // 공격 중 상태 유지 시간 ( >0 이면 공격 중)

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        anim.applyRootMotion = false;
    }

    void Update()
    {
        HandleMovement();
        HandleAttack();

        // 공격 락 타이머 감소
        if (attackLockTimer > 0f)
            attackLockTimer -= Time.deltaTime;
    }

    // --- 이동 & 회전 ---
    void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        bool isRunKey = Input.GetKey(KeyCode.LeftShift);

        // 카메라 기준 방향
        Transform cam = Camera.main.transform;
        Vector3 camForward = cam.forward; camForward.y = 0; camForward.Normalize();
        Vector3 camRight = cam.right; camRight.y = 0; camRight.Normalize();

        Vector3 inputDir = (camForward * v + camRight * h);
        inputDir = Vector3.ClampMagnitude(inputDir, 1f);

        bool isAttacking = attackLockTimer > 0f;

        // ★ 공격 중에는 이동 속도 슬로우
        float baseSpeed = isRunKey ? runSpeed : walkSpeed;
        float speedFactor = isAttacking ? attackMoveSlowFactor : 1f;
        float finalSpeed = baseSpeed * speedFactor;

        Vector3 move = inputDir * finalSpeed;

        // ★ 평소에만 이동 방향으로 회전, 공격 중에는 공격 시 맞춘 방향 유지
        if (!isAttacking && move.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(inputDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotateSpeed * Time.deltaTime
            );
        }

        controller.Move(move * Time.deltaTime);

        // 중력
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Blend Tree 파라미터
        float normalizedSpeed = move.magnitude / runSpeed; // 0~1
        anim.SetFloat("MoveSpeed", normalizedSpeed, 0.1f, Time.deltaTime);
    }

    // --- 공격 (공격 순간에만 마우스 방향 회전 + 연타 큐 방지) ---
    void HandleAttack()
    {
        if (attackTimer > 0f)
            attackTimer -= Time.deltaTime;

        bool attackPressed = Input.GetMouseButtonDown(0);

        // 쿨다운 끝났을 때만 공격 1회
        if (attackPressed && attackTimer <= 0f)
        {
            // 1) 공격 방향 = 마우스 바라보는 방향으로 한 번 딱 돌림
            Vector3 mouseDir;
            if (TryGetMouseDirection(out mouseDir))
            {
                Quaternion targetRot = Quaternion.LookRotation(mouseDir);
                transform.rotation = targetRot;   // 여기서는 순간적으로 딱 맞춤
            }

            // 2) 애니메이션 발동
            attackTimer = attackCooldown;
            attackLockTimer = attackLockTime;    // 이 시간 동안 이동 느려지고 방향 고정
            anim.SetTrigger("Attack");
        }
    }

    // 마우스가 바라보는 바닥 방향 구하기
    bool TryGetMouseDirection(out Vector3 dir)
    {
        dir = Vector3.zero;

        Camera cam = Camera.main;
        if (cam == null) return false;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));

        float enter;
        if (groundPlane.Raycast(ray, out enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            dir = hitPoint - transform.position;
            dir.y = 0;

            if (dir.sqrMagnitude > 0.001f)
            {
                dir.Normalize();
                return true;
            }
        }

        return false;
    }
}
