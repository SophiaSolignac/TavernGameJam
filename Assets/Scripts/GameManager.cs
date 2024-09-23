using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<FurnitureData> furnitureDatas = new List<FurnitureData>();
    private int _CurrentIndex = 0;
    public int _CurrentYRotation { private set; get; } = 0;

    public FurnitureData LoadObject(int pObjectIndexModifier)
    {
        _CurrentIndex += pObjectIndexModifier;
        if (_CurrentIndex >= furnitureDatas.Count) _CurrentIndex -= furnitureDatas.Count;
        if (_CurrentIndex < 0) _CurrentIndex += furnitureDatas.Count;
        return furnitureDatas[_CurrentIndex];
    }

    public void RotateObjects()
    {
        _CurrentYRotation = (_CurrentYRotation + 90) % 360;
        FurnitureData lFurnitureData;
        for (int i = 0; i < furnitureDatas.Count; i++)
        {
            lFurnitureData = furnitureDatas[i];
            if (i != furnitureDatas.IndexOf(lFurnitureData)) continue;
            lFurnitureData.size = new Vector3(lFurnitureData.size.z, lFurnitureData.size.y, lFurnitureData.size.x);
        }
    }
}
