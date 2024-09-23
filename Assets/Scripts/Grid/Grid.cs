using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private Vector3Int _NewGridSize = new(10, 5, 10);
    [SerializeField] private float _CellSize = 1.0f;

    [SerializeField] private Cell _Cell = default;
    private Cellule[,,] _NewGrid;

    void Start()
    {
        // Initialize the grid
        CreateGrid();
    }

    private void CreateGrid()
    {
        _NewGrid = new Cellule[_NewGridSize.x, _NewGridSize.y, _NewGridSize.z];

        for (int y = 0; y < _NewGrid.GetLength(1); y++)
            for (int x = 0; x < _NewGrid.GetLength(0); x++)
                for (int z = 0; z < _NewGrid.GetLength(2); z++)
                {
                    _NewGrid[x, y, z] = new Cellule();
                    TryDisplayVisual(new Vector3Int(x, y, z));
                }
    }

    private void TryDisplayVisual(Vector3Int pCellToDisplay)
    {
        Vector3Int lCellUnder = new Vector3Int(pCellToDisplay.x, pCellToDisplay.y - 1, pCellToDisplay.z);
        Vector3 lCellPosition = new Vector3(pCellToDisplay.x * _CellSize - (_NewGridSize.x - 1) / 2f * _CellSize,
                                            pCellToDisplay.y * _CellSize,
                                            pCellToDisplay.z * _CellSize - (_NewGridSize.z - 1) / 2f * _CellSize);

        Cell lVisualCell = Instantiate(_Cell, lCellPosition, Quaternion.identity, transform);
        if (pCellToDisplay.x < 3 && pCellToDisplay.y == 0 && pCellToDisplay.z < 3)
            _NewGrid[pCellToDisplay.x, pCellToDisplay.y, pCellToDisplay.z].FillEmptyCell();

        lVisualCell.name = $"Cellule_{pCellToDisplay.x}_{pCellToDisplay.y}_{pCellToDisplay.z}";
        _NewGrid[pCellToDisplay.x, pCellToDisplay.y, pCellToDisplay.z].gridPreview = lVisualCell;
        lVisualCell.ToggleVisual();

        if (!CellCanContain(pCellToDisplay))
            lVisualCell.gameObject.SetActive(false);
    }

    public bool CellExist(Vector3Int pCellToCheck)
    {
        return pCellToCheck.x.IsBetween(0, _NewGrid.GetLength(0) - 1)
            && pCellToCheck.y.IsBetween(0, _NewGrid.GetLength(1) - 1)
            && pCellToCheck.z.IsBetween(0, _NewGrid.GetLength(2) - 1);
    }

    public bool CellEmpty(Vector3Int pCellToCheck)
    {
        if (!CellExist(pCellToCheck))
            return false;

        return AccessToCell(pCellToCheck).furniture == null;
    }

    public bool CellCanContain(Vector3Int pCellToCheck)
    {
        if (!CellExist(pCellToCheck))
            return false;

        Vector3Int lCellUnder = new Vector3Int(pCellToCheck.x, pCellToCheck.y - 1, pCellToCheck.z);
        return CellEmpty(pCellToCheck) && (!CellExist(lCellUnder) || (!CellEmpty(lCellUnder) && CellCanHaveOnTop(lCellUnder)));
    }

    public bool CellCanHaveOnTop(Vector3Int pCellToCheck)
    {
        if (!CellExist(pCellToCheck) || CellEmpty(pCellToCheck))
            return false;

        return AccessToCell(pCellToCheck).furniture.canHaveOnTop;
    }

    public Cellule AccessToCell(Vector3Int pCellToAccess)
    {
        if (!CellExist(pCellToAccess))
            return null;

        return _NewGrid[pCellToAccess.x, pCellToAccess.y, pCellToAccess.z];
    }

    public void AddObjectToCell(Vector3Int pCellToAdd, FurnitureData pFurnitureToAdd)
    {

    }

    public void ToggleVisibility()
    {
        foreach (Cellule lCell in _NewGrid)
            lCell.gridPreview.ToggleVisual();
    }
}
public class Cellule
{
    // Tableau pour stocker des objets GameObject
    public Cell gridPreview;
    public FurnitureData furniture = null;

    public Cellule()
    {
    }

    public void AddObject(FurnitureData pFurniture)
    {
        furniture = pFurniture;
    }

    public FurnitureData RemoveObject()
    {
        FurnitureData lFurniture = furniture;
        furniture = null;
        return lFurniture;
    }

    public void FillEmptyCell()
    {
        AddObject(new FurnitureData());
    }
}