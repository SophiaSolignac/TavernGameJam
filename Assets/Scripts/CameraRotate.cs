using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraRotate : MonoBehaviour
{
    [SerializeField]
    Vector3 center;
    Vector3 cameraArm;
    // Start is called before the first frame update
    void Start()
    {
        cameraArm = transform.position + center;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (center != null)
        {
            Gizmos.DrawSphere(center + transform.position,0.5f);
        }
        Gizmos.DrawLine(center + transform.position, transform.position);
    }

}
