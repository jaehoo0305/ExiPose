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
        if (!ValidateProbs())
        {
            Debug.LogWarning("[ItemDropper] 프리팹이 누락되었거나 설정이 올바르지 않습니다.");
            return;
        }

        int index = SampleIndex(probs);
        if (index == -1) return;

        Vector3 pos = dropPoint != null ? dropPoint.position : transform.position;

        Instantiate(probs[index].prefab, pos, Quaternion.identity);
    }

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

        float[] prefix = new float[n];
        float total = 0f;

        for (int i = 0; i < n; i++)
        {
            total += weights[i].weight;
            prefix[i] = total;
        }

        float r = Random.Range(0f, total);

        int left = 0;
        int right = n - 1;

        while (left < right)
        {
            int mid = (left + right) / 2;

            if (prefix[mid] < r)
            { 
                left = mid + 1;
            }
            else
            {
                right = mid;
            }
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