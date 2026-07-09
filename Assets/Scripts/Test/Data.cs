using System;
using UnityEngine;

[Serializable]
public class Data
{
    public string Name { get; set; }
    public int Age { get; set; }
    public float Height { get; set; }

    public override string ToString()
    {
        return $"{Name}({Age}): {Height}";
    }
}