using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Furniture", menuName = "Furniture")]
public class FurnitureData : ScriptableObject
{
    public GameObject prefab = default;
    public Vector3 size = Vector3.one;
    public float stressValue = 0f;
    public bool canHaveOnTop = false;
    public bool canBeOnTop = false;
}
