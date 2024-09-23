using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private MeshRenderer _MR;

    private void Awake()
    {
        _MR = GetComponent<MeshRenderer>();
    }

    public void ToggleVisual()
    {
        //Color lNewColor = _MR.material.color;
        //lNewColor.a = pVisible ? .5f : 0;
        //_MR.material.color = lNewColor;
        _MR.enabled = !_MR.enabled;
    }
}
