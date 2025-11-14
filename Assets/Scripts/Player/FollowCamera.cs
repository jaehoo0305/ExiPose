using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;          // 따라갈 플레이어 (man_casual)

    [Header("Offset")]
    public Vector3 offset = new Vector3(-7f, 7f, 0f); // 플레이어 기준 위치
    public float followSpeed = 5f;    // 카메라가 따라오는 속도 (선형 보간 계수)

    [Header("Rotation")]
    public bool lockRotation = true;  // 고정 각도 쓸지
    public Vector3 fixedEuler = new Vector3(45f, 90f, 0f); // 좀보이드 느낌 각도

    void Start()
    {
        if (lockRotation)
            transform.rotation = Quaternion.Euler(fixedEuler);
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 목표 위치 = 플레이어 위치 + 오프셋
        Vector3 desiredPos = target.position + offset;

        // 선형 보간 (Lerp)으로 부드럽게 따라가기
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPos,
            followSpeed * Time.deltaTime
        );
    }
}
