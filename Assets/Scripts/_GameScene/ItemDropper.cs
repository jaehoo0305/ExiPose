using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    [Header("Drop Prefabs")]
    public GameObject commonPrefab;  // 50%
    public GameObject rarePrefab;    // 35%
    public GameObject epicPrefab;    // 15%

    [Header("Drop Area")]
    public Transform dropPoint;      // 드롭될 위치 (없으면 자기 위치 사용)

    // 내부에서 쓸 확률 배열
    float[] probs;
    GameObject[] prefabs;

    void Awake()
    {
        // 확률 배열과 프리팹 배열 구성
        probs = new float[3];
        probs[0] = 0.5f;   // common
        probs[1] = 0.35f;  // rare
        probs[2] = 0.15f;  // epic

        prefabs = new GameObject[3];
        prefabs[0] = commonPrefab;
        prefabs[1] = rarePrefab;
        prefabs[2] = epicPrefab;
    }

    void Start()
    {
        DropItem();
    }

    // 아이템 한 개 드롭
    public void DropItem()
    {
        if (!ValidatePrefabs())
        {
            Debug.LogWarning("[ItemDropper] Prefab is missing.");
            return;
        }

        int index = SampleIndex(probs);
        Vector3 pos = dropPoint != null ? dropPoint.position : transform.position;

        Instantiate(prefabs[index], pos, Quaternion.identity);
    }

    bool ValidatePrefabs()
    {
        return commonPrefab != null && rarePrefab != null && epicPrefab != null;
    }

    // Categorical Sampling (Prefix Sum + Binary Search)
    int SampleIndex(float[] weights)
    {
        int n = weights.Length;
        if (n == 0) return -1;

        float[] prefix = new float[n];
        float total = 0f;

        for (int i = 0; i < n; i++)
        {
            total += weights[i];
            prefix[i] = total;
        }

        if (total <= 0f)
            return -1;

        float r = Random.Range(0f, total);

        int left = 0;
        int right = n - 1;

        while (left < right)
        {
            int mid = (left + right) / 2;

            if (prefix[mid] < r)
                left = mid + 1;
            else
                right = mid;
        }

        return left;
    }

    // 에디터에서 테스트용
    [ContextMenu("Test Drop")]
    void TestDropInEditor()
    {
        DropItem();
    }
}
