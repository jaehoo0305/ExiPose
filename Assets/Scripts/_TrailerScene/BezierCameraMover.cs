using UnityEngine;

public class BezierCameraMover : MonoBehaviour
{
    [Header("Control Points")]
    public Transform startPoint;
    public Transform controlPoint1;
    public Transform controlPoint2;
    public Transform endPoint;

    [Header("Move Settings")]
    public float duration = 5f;
    public bool playOnStart = true;

    float time = 0f;
    bool isPlaying;

    public void Play()
    {
        time = 0f;
        isPlaying = true;
    }

    void Update()
    {
        time += Time.deltaTime / duration;

        transform.position = bezierCurve();
    }

    Vector3 bezierCurve()
    {
        Vector3 p1 = Vector3.Lerp(startPoint.position, 
                                  controlPoint1.position, time);
        Vector3 p2 = Vector3.Lerp(controlPoint1.position, 
                                  controlPoint2.position, time);
        Vector3 p3 = Vector3.Lerp(controlPoint2.position, 
                                  endPoint.position, time);
        Vector3 p4 = Vector3.Lerp(p1, p2, time);
        Vector3 p5 = Vector3.Lerp(p2, p3, time);
        Vector3 p6 = Vector3.Lerp(p4, p5, time);

        return p5;
    }

    public void PlayFromTimeline()
    {
        Play();
    }
}
