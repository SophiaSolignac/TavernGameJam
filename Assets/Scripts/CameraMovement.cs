using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum CamPositions {Living, Bedroom, Kitchen }
public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Transform[] positions = new Transform[3];

    private int currentPosition = 0;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MoveCam(CamPositions.Living);
        }
        if (Input.GetMouseButtonDown(1))
        {
            MoveCam(CamPositions.Kitchen);
        }
        if (Input.GetMouseButtonDown(2))
        {
            MoveCam(CamPositions.Bedroom);
        }
    }

    private void MoveCam(CamPositions pCamPositions)
    {
        Transform lNewPosition = positions[(int)pCamPositions];
        ((Light)positions[currentPosition].transform.GetChild(0).GetComponent("Light")).enabled = false; //Switch the light off in the current room
        transform.position = lNewPosition.position;
        transform.rotation = lNewPosition.rotation;
        currentPosition = (int)pCamPositions;
        ((Light)lNewPosition.transform.GetChild(0).GetComponent("Light")).enabled = true; // switch the light On in the new room
    }
}
