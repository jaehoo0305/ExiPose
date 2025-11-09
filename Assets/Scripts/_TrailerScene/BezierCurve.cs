using UnityEngine;

[System.Serializable]
public class BezierCurve
{
    public Vector3 p0; // 시작점
    public Vector3 p1; // 첫 번째 컨트롤 포인트
    public Vector3 p2; // 두 번째 컨트롤 포인트
    public Vector3 p3; // 끝점

    // t: 0~1
    public Vector3 Evaluate(float t)
    {
        // 큐빅 베지어 공식
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
}
