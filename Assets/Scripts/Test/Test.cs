using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Test : MonoBehaviour
{
    // [TestMethod] 제거 (유니티 컴파일러가 인식하지 못하는 어트리뷰트)
    private void TestMethod()
    {
        string key = "AIzaSyBjsVdVj3cQ0N3D6sA4RhyjYZnJ1D8Q7nk";
        string path = "1mmRlaEo1MMecBP_ZtqU9I_rnPMajg3C44q53LeUh2J0";
        string sheet = "Test";

        // SpreadSheet / CSV 라이브러리가 없는 경우 임시 주석 처리
        /*
        var data = SpreadSheet.LoadData(path, sheet, key);
        var result = CSV.DeserializeToList<Data>(data) as List<Data>;
        Debug.Log(string.Join('\n', result));
        */
    }
}