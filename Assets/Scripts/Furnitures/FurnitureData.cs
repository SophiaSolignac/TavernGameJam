using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Furniture", menuName = "Furniture")]
public class FurnitureData : ScriptableObject
{
    [SerializeField] private Texture _VisualAsset = null;
    [SerializeField] private Vector3 _Size = Vector3.one;
    [SerializeField] private float _StressValue = 0f;
    [SerializeField] private bool _CanHaveOnTop = false;
    [SerializeField] private bool _CanBeOnTop = false;
}
