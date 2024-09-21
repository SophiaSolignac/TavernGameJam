using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    
    public int _GridWidth = 10;
    public int _GridHeight = 10;
    public float _CellSize = 1.0f;
    public int _LeyerPerCell = 5;

    private Cellule[,] _Grid;

    void Start()
    {
        // Initialize the grid
        CreateGrid();

        //PutAnObjectOnAGrid(2, 2, GameObject.CreatePrimitive(PrimitiveType.Cube));
    }

    private void CreateGrid()
    {
        _Grid = new Cellule[_GridWidth, _GridHeight];
        Vector3 cellPosition;
        GameObject visualCell;

        for (int x = 0; x < _GridWidth; x++)
        {
            for (int z = 0; z < _GridHeight; z++)
            {
                _Grid[x, z] = new Cellule(_LeyerPerCell);

                // Visual representation of the grid
                cellPosition = new Vector3(x * _CellSize - _GridWidth / 2 * _CellSize, 0, z * _CellSize - _GridHeight / 2 * _CellSize);

                visualCell = GameObject.CreatePrimitive(PrimitiveType.Cube);
                visualCell.transform.SetParent(transform);
                visualCell.transform.position = cellPosition;
                visualCell.transform.localScale = new Vector3(_CellSize, 0.1f, _CellSize);
                visualCell.GetComponent<MeshRenderer>().material.color = (x + z) % 2 == 1 ? Color.grey : Color.white;

                _Grid[x, z].objets[0] = visualCell;
                visualCell.name = $"Cellule_{x}_{z}";
            }
        }
    }

    // Check the existence of a cell
    public bool cellExist(int x, int z)
    {
        if (x >= 0 && x < _GridWidth && z >= 0 && z < _GridHeight) return true;
        return false;
    }

    public Cellule AccessToCell(int x, int z)
    {
        if (cellExist(x, z)) return _Grid[x, z];
        else return _Grid[0, 0];
    }

    public void PutAnObjectOnAGrid(int x, int z, GameObject pObject)
    {
        if (cellExist(x, z))
        {
            _Grid[x, z].objets[1] = pObject;
            _Grid[x, z].objets[1].transform.position = _Grid[x, z].objets[0].transform.position;
        }
    }
}
public class Cellule
{
    // Tableau pour stocker des objets GameObject
    public GameObject[] objets;

    // Constructeur qui permet de sp�cifier la taille du tableau
    public Cellule(int tailleArray)
    {
        objets = new GameObject[tailleArray];  // Initialise un tableau de GameObjects
    }
}