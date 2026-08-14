using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    [System.Serializable]
    public struct ItemElement
    {
        public string itemName;
        public float weight;
        public GameObject prefab;
    }

    [Header("Weights Setting")]
    public ItemElement[] probs;

    [Header("Drop Area")]
    public Transform dropPoint;

    void Start()
    {
        DropItem();
    }

    public void DropItem()
    {
        // 1. 유효성 검사 함수 이름 및 대상 변경
        if (!ValidateProbs())
        {
            Debug.LogWarning("[ItemDropper] 프리팹이 누락되었거나 설정이 올바르지 않습니다.");
            return;
        }

        int index = SampleIndex(probs);
        if (index == -1) return;

        Vector3 pos = dropPoint != null ? dropPoint.position : transform.position;

        // 2. 기존 prefabs[index] 대신 probs[index].prefab 사용
        Instantiate(probs[index].prefab, pos, Quaternion.identity);
    }

    // 3. probs 배열 내부에 프리팹이 잘 들어있는지 검사하도록 수정
    bool ValidateProbs()
    {
        if (probs == null || probs.Length == 0) return false;
        for (int i = 0; i < probs.Length; i++)
        {
            if (probs[i].prefab == null) return false;
        }
        return true;
    }

    int SampleIndex(ItemElement[] weights)
    {
        int n = weights.Length;

        if (n == 0) return -1;

        float[] prefix = new float[n];
        float total = 0f;

        for (int i = 0; i < n; i++)
        {
            total += weights[i].weight;
            prefix[i] = total;
        }

        if (total <= 0f) return -1;

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