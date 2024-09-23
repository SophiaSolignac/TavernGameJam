using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PrintCurve : MonoBehaviour
{
    [SerializeField,Range(0.001f,10f)]
    private float radius = 0.2f;
    [SerializeField]
    Color Color = Color.white;
    [SerializeField]
    int nbPoints = 10;
    [SerializeField, Range(1,10)]
    float Spacing;
    [SerializeField]
    Easing.EaseType EaseType= Easing.EaseType.None;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color;

        for (int i = 0; i < nbPoints; i++)
        {
            float ratio = (float)i / nbPoints;
            Vector3 pos = new Vector3(ratio,0,0);
            pos.y = Easing.Ease(ratio, EaseType);
            pos *= Spacing;
            pos += Vector3.forward;
            Gizmos.DrawCube(pos, Vector3.one *radius);
        }
    }
}
