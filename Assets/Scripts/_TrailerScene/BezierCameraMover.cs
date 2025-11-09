using UnityEngine;

public class BezierCameraMover : MonoBehaviour
{
    [Header("Control Points")]
    public Transform startPoint;     // 빌딩 하단 근처
    public Transform controlPoint1;  // 벽에서 조금 떨어진 곳
    public Transform controlPoint2;  // 위쪽 창가 근처
    public Transform endPoint;       // 옥상 위 or 옥상 위 약간 뒤

    [Header("Move Settings")]
    public float duration = 5f;      // 몇 초 동안 올라갈지
    public bool playOnStart = true;
    public bool lookAtTarget = true; // 올라가면서 빌딩 바라보기
    public Transform lookTarget;     // 보통 빌딩 중심이나 옥상 오브젝트

    float _time;
    bool _isPlaying;

    void Start()
    {
        if (playOnStart)
            Play();
    }

    public void Play()
    {
        _time = 0f;
        _isPlaying = true;
    }

    void Update()
    {
        if (!_isPlaying) return;
        if (duration <= 0f) return;

        _time += Time.deltaTime;
        float t = Mathf.Clamp01(_time / duration);

        // 베지어 계산
        Vector3 p0 = startPoint.position;
        Vector3 p1 = controlPoint1.position;
        Vector3 p2 = controlPoint2.position;
        Vector3 p3 = endPoint.position;

        Vector3 pos = EvaluateBezier(p0, p1, p2, p3, t);
        transform.position = pos;

        if (lookAtTarget && lookTarget != null)
        {
            Vector3 dir = (lookTarget.position - transform.position).normalized;
            if (dir.sqrMagnitude > 0.0001f)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(dir, Vector3.up),
                    Time.deltaTime * 5f
                );
            }
        }

        if (t >= 1f)
        {
            _isPlaying = false;
        }
    }

    // 이 스크립트 안에도 베지어 넣어두자
    Vector3 EvaluateBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1f - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 point =
            uuu * p0 +
            3f * uu * t * p1 +
            3f * u * tt * p2 +
            ttt * p3;

        return point;
    }

    public void PlayFromTimeline()
    {
        Play();   // 기존에 있던 거 호출
    }
}
