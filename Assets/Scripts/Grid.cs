using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int _GridWith = 10;
    public int _GridHeight = 10;
    public float _CellSize = 1.0f;
    public int _LeyerPerCell = 5;

    private Cellule[,] _Grid;

    public class Cellule
    {
        // Tableau pour stocker des objets GameObject
        public GameObject[] objets;

        // Constructeur qui permet de spécifier la taille du tableau
        public Cellule(int tailleArray)
        {
            objets = new GameObject[tailleArray];  // Initialise un tableau de GameObjects
        }
    }

    void Start()
    {
        // Initialize the grid
        _Grid = new Cellule[_GridWith, _GridHeight];

        Vector3 cellPosition;
        GameObject visualCell;

        for (int x = 0; x < _GridWith; x++)
        {
            for (int z = 0; z < _GridHeight; z++)
            {
                _Grid[x, z] = new Cellule(_LeyerPerCell);

                // Visual representation of the grid
                cellPosition = new Vector3(x * _CellSize, 0, z * _CellSize);

                visualCell = GameObject.CreatePrimitive(PrimitiveType.Cube);

                visualCell.transform.position = cellPosition;
                visualCell.transform.localScale = new Vector3(_CellSize, 0.1f, _CellSize);

                _Grid[x, z].objets[0] = visualCell;
                visualCell.name = $"Cellule_{x}_{z}";
            }
        }

        //PutAnObjectOnAGrid(2, 2, GameObject.CreatePrimitive(PrimitiveType.Cube));

    }

    // Check the existence of a cell

    public bool cellExist(int x, int z)
    {
        if (x >= 0 && x < _GridWith && z >= 0 && z < _GridHeight) return true;
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
