using CSVData;
using CSVData.Extensions;
using Extension.Test;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Test : MonoBehaviour
{
    [TestMethod]
    private void TestMethod()
    {
        string key = "AIzaSyBjsVdVj3cQ0N3D6sA4RhyjYZnJ1D8Q7nk";
        string path = "1mmRlaEo1MMecBP_ZtqU9I_rnPMajg3C44q53LeUh2J0";
        string sheet = "Test";
        var data = SpreadSheet.LoadData(path, sheet, key);
        var result = CSV.DeserializeToList<Data>(data) as List<Data>;
        Debug.Log(string.Join('\n', result));
    }
}
