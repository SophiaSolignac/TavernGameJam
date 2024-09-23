using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class FurniturePreview : MonoBehaviour
{
    [SerializeField] private Grid _Grid;
    [SerializeField] private LayerMask _LayersToHit;
    private Transform _PreviousHoveredCell = null;

    //private MeshRenderer _mr;
    private Vector3 _Size;
    private Vector3 _CenterOffset = Vector3.zero;
    private Plane plane = new(Vector3.down, 0);

    private Regex _RegexCellName = new(@"Cellule_(\d+)_(\d+)_(\d+)");

    private GameManager _GM;
    private FurnitureData _CurrentFurnitureData;
    private GameObject _CurrentFurniturePrefab;

    // SHOULD BE IN GAME MANAGER
    private List<Vector3> _ObjectsToPosition = new List<Vector3>();
    private int _ObjectIndex = 0;

    private void Awake()
    {
        //_mr = GetComponent<MeshRenderer>();
        //_mr.material.color = Color.red;

        // TO REMOVE ONCE WE HAVE GAME MANAGER
        _ObjectsToPosition.Add(new Vector3(1, 1, 1));
        _ObjectsToPosition.Add(new Vector3(2, 1, 3));
        _ObjectsToPosition.Add(new Vector3(3, 3, 3));

        _GM = FindFirstObjectByType<GameManager>();

        LoadObject(0);
    }

    private void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
            LoadObject((int)Input.mouseScrollDelta.y);
        if (Input.GetMouseButtonDown(1))
            RotateObjects();
        if (Input.GetMouseButtonDown(0))
            DropObject();
        if (Input.GetMouseButtonDown(2))
            ToggleGrid();

        DetectObjectUnderMouse();
    }

    private void LoadObject(int pObjectIndexModifier)
    {
        foreach (Transform lChild in transform)
            Destroy(lChild.gameObject);

        _CurrentFurnitureData = _GM.LoadObject(pObjectIndexModifier);
        _CurrentFurniturePrefab = Instantiate(_CurrentFurnitureData.prefab);
        _CurrentFurniturePrefab.transform.position = transform.position;
        _CurrentFurniturePrefab.transform.parent = transform;
        _CurrentFurniturePrefab.transform.Rotate(Vector3.up, _GM._CurrentYRotation, 0);

        _CenterOffset = new Vector3((_CurrentFurnitureData.size.x - 1) % 2 / 2, (_CurrentFurnitureData.size.y - 1) / 2, (_CurrentFurnitureData.size.z - 1) % 2 / 2);
        _PreviousHoveredCell = null;
    }

    private void RotateObjects()
    {
        _GM.RotateObjects();
        LoadObject(0);

        // APPLIQUEZ SUR LE SPRITE
        //transform.Rotate(0, 90, 0);
    }

    private void DropObject()
    {
        //if (_mr.material.color == Color.red) return;
    }

    private void ToggleGrid()
    {
        _Grid.ToggleVisibility();
    }

    private void DetectObjectUnderMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;

        if (Physics.Raycast(ray, out RaycastHit hit, 100, _LayersToHit) && hit.collider.gameObject.TryGetComponent(out Transform hoveredObject))
        {
            //if (IsShapeInGrid(_ObjectsToPosition[_ObjectIndex], hoveredObject))
            //if (IsShapeInGrid(_CurrentFurnitureData.size, hoveredObject))
            //    _mr.material.color = Color.green;
            //else
            //    _mr.material.color = Color.red;

            if (hoveredObject != _PreviousHoveredCell)
            {
                _PreviousHoveredCell = hoveredObject;
                transform.position = hoveredObject.position + _CenterOffset;
                Debug.Log(transform.position);
            }
        }
        else 
        {
            if (_PreviousHoveredCell != null)
            {
                //_mr.material.color = Color.red;
                _PreviousHoveredCell = null;
            }
            if (plane.Raycast(ray, out distance))
            {
                transform.position = ray.GetPoint(distance);
            }
        }
    }

    private bool IsShapeInGrid(Vector3 pObjectSize, Transform pCellCenter)
    {
        int lCellToCheckXPos = Mathf.CeilToInt((pObjectSize.x - 1) / 2f);
        int lCellToCheckXNeg = Mathf.FloorToInt((pObjectSize.x - 1) / 2f);
        int lCellToCheckZPos = Mathf.CeilToInt((pObjectSize.z - 1) / 2f);
        int lCellToCheckZNeg = Mathf.FloorToInt((pObjectSize.z - 1) / 2f);

        return CheckInDirection(Vector3Int.right, lCellToCheckXPos, pCellCenter)
            && CheckInDirection(Vector3Int.left, lCellToCheckXNeg, pCellCenter)
            && CheckInDirection(Vector3Int.forward, lCellToCheckZPos, pCellCenter)
            && CheckInDirection(Vector3Int.back, lCellToCheckZNeg, pCellCenter);
    }

    private bool CheckInDirection(Vector3Int pDirection, int pCountInDirection, Transform pOrigin)
    {
        if (pCountInDirection == 0) return true;


        Match pSplittedName = _RegexCellName.Match(pOrigin.name);
        int lX = int.Parse(pSplittedName.Groups[1].Value);
        int lY = int.Parse(pSplittedName.Groups[2].Value);
        int lZ = int.Parse(pSplittedName.Groups[3].Value);

        for (int i = 1; i <= pCountInDirection; i++)
        {
            if (!_Grid.CellCanContain(new Vector3Int(lX + pDirection.x * i, lY, lZ + pDirection.z * i)))
                return false;
        }

        return true;
    }
}