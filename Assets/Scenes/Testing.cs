using UnityEngine;

public class Testing : MonoBehaviour
{
    public Transform pointStart;
    public Transform point1;
    public Transform point2;
    public Transform pointEnd;
    public float duration = 5f;
    float time = 0f;

    private void Update()
    {
        if (time > 1f)
        {
            time = 0f;
        }

        time += Time.deltaTime / duration;

        transform.position = bezierdCurve(); 
    }

    Vector3 bezierdCurve()
    {
        Vector3 p1 = Vector3.Lerp(pointStart.position, point1.position, time);
        Vector3 p2 = Vector3.Lerp(point1.position, point2.position, time);
        Vector3 p3 = Vector3.Lerp(point2.position, pointEnd.position, time);
        Vector3 p4 = Vector3.Lerp(p1, p2, time);
        Vector3 p5 = Vector3.Lerp(p2, p3, time);
        Vector3 p6 = Vector3.Lerp(p4, p5, time); //야르 되요 된다구요

        return p6;
    }
}
