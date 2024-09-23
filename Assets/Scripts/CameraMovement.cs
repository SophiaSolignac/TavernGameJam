using _Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


enum CamPositions {Living, Bedroom, Kitchen }

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Transform[] positions = new Transform[3];
    [SerializeField]
    private Transform[] rails = new Transform[3];
    MoveCube MoveCube;
    PerspectiveSwitcher PerspectiveSwitcher;
    [SerializeField]
    private CamPositions currentPosition = CamPositions.Living;

    delegate void CamAction();

    CamAction camAction;

    [SerializeField]
    Transform path;
    [SerializeField]
    Easing.EaseType easeType = Easing.EaseType.None;
    private float timer = 0;
    [SerializeField]
    float duration = 5f;
    private bool inverse = false;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0; 
        DoWait(); 
        PerspectiveSwitcher = GetComponent<PerspectiveSwitcher>();
    }

    private void DoMove()
    {
        timer = 0;
        camAction = Move;
    }

    private void Move()
    {
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            timer = 0;
            return;
        }
        if (inverse == true)
        {
            path.GetComponent<SlerpySlerp>().ApplyMovementInverse(transform, duration - timer, duration, easeType);
        }
        else
        {
            path.GetComponent<SlerpySlerp>().ApplyMovement(transform, timer, duration, easeType);
        }
    }
    private void DoWait()
    {
        inverse = false;
        camAction = Wait;
    }

    private void Wait()
    {

    }
    private void DoZoom()
    {
        camAction = Zoom;
    }

    private void Zoom()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleInputs();
        camAction();
    }

    private void HandleInputs()
    {
        if (camAction != Wait) return;
        if (Input.GetMouseButtonDown(0))  // Living
        {
            if (currentPosition == CamPositions.Living) return;

            if (currentPosition == CamPositions.Kitchen) path = rails[2];
            if (currentPosition == CamPositions.Bedroom) { inverse = true; path = rails[0]; }

            StartCoroutine(MoveCam(CamPositions.Living, PerspectiveSwitcher.Duration));
        }
        if (Input.GetMouseButtonDown(1))  // Bedroom
        {
            if (currentPosition == CamPositions.Bedroom) return;
            if (currentPosition == CamPositions.Living) path = rails[0];
            if (currentPosition == CamPositions.Kitchen) { inverse = true; path = rails[1]; }

            StartCoroutine(MoveCam(CamPositions.Bedroom, PerspectiveSwitcher.Duration));
        }
        if (Input.GetMouseButtonDown(2))  // Kitchen
        {
            if (currentPosition == CamPositions.Kitchen) return;

            if (currentPosition == CamPositions.Bedroom) path = rails[1];
            if (currentPosition == CamPositions.Living) { inverse = true; path = rails[2]; };

            StartCoroutine(MoveCam(CamPositions.Kitchen, PerspectiveSwitcher.Duration));
        }
    }

    private void MoveCam(CamPositions pCamPositions)
    {
        Transform lNewPosition = positions[(int)pCamPositions];
        ((Light)positions[(int)currentPosition].transform.GetChild(0).GetComponent("Light")).enabled = false; //Switch the light off in the current room
        DoMove();
        currentPosition = pCamPositions;
        ((Light)lNewPosition.transform.GetChild(0).GetComponent("Light")).enabled = true; // switch the light On in the new room
    }
    private IEnumerator MoveCam(CamPositions pCamPositions,float pWaitDuration)
    {
        DoZoom();
        PerspectiveSwitcher.SwitchPerspective();
        yield return new WaitForSeconds(pWaitDuration);
        MoveCam(pCamPositions);
        yield return new WaitForSeconds(duration);
        DoZoom();
        PerspectiveSwitcher.SwitchPerspective();
        yield return new WaitForSeconds(pWaitDuration);
        DoWait(); 
    }
}
