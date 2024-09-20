using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellDetection : MonoBehaviour
{
    void Update()
    {
        DetectObjectUnderMouse();
    }

    void DetectObjectUnderMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        GameObject hoveredObject;

        if (Physics.Raycast(ray, out hit))
        {
            hoveredObject = hit.collider.gameObject;

            Debug.Log("Objet survolé: " + hoveredObject.name);

            hoveredObject.GetComponent<Renderer>().material.color = Color.red;
        }
    }
}
