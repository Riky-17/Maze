using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] MazeCell mazeCellPrefab;
    Vector2 offset;
    [SerializeField] float mazeWidth;
    [SerializeField] float mazeHeight;
    int cellsAmountX => Mathf.FloorToInt(mazeWidth) / mazeCellDiameter;
    int cellsAmountY => Mathf.FloorToInt(mazeHeight) / mazeCellDiameter;
    int mazeCellRadius = 1;
    int mazeCellDiameter => mazeCellRadius * 2;
    MazeCell[,] mazeGrid;
    
    Stack<MazeCell> currentPath;
    List<MazeCell> completedPath;

    void Awake()
    {
        offset = new(0, mazeHeight / 2 + 5);
        mazeGrid = new MazeCell[cellsAmountX, cellsAmountY];
        currentPath = new(cellsAmountX * cellsAmountY);
        completedPath = new(cellsAmountX * cellsAmountY);
    }

    void Start()
    {
        CreateGrid();
        GenerateMaze();
    }

    void CreateGrid()
    {
        Vector2 bottomLeftCorner = new(-cellsAmountX, -cellsAmountY);

        for (int x = 0; x < cellsAmountX; x++)
        {
            for (int y = 0; y < cellsAmountY; y++)
            {
                float xPos = x * mazeCellDiameter + mazeCellRadius;
                float yPos = y * mazeCellDiameter + mazeCellRadius;
                Vector2 cellPos = new Vector2(xPos, yPos) + bottomLeftCorner + offset;

                MazeCell newCell = Instantiate(mazeCellPrefab, cellPos, Quaternion.identity);
                (newCell.x, newCell.y) = (x, y);
                mazeGrid[x, y] = newCell;
            }
        }
    }

    void GenerateMaze()
    {
        MazeCell currentCell;
        currentCell = mazeGrid[0, 0];
        currentPath.Push(currentCell);
        List<MazeCell> neighboursList;
        Dictionary<MazeCell, int> neighboursDic;

        while(currentPath.Count > 0)
        {
            (neighboursList, neighboursDic) = GetNeighbours(currentCell);
            if (neighboursList.Count > 0)
            {
                currentCell = neighboursList[Random.Range(0, neighboursList.Count)];
                neighboursDic.TryGetValue(currentCell, out int wallDir);
                int oppositeWall = wallDir + 2 < 4 ? wallDir + 2 : wallDir - 2;
                RemoveWalls(currentCell, oppositeWall);
                RemoveWalls(currentPath.Peek(), wallDir);
                currentPath.Push(currentCell);
            }
            else
            {
                completedPath.Add(currentPath.Pop());
                if(currentPath.Count == 0)
                    break;

                currentCell = currentPath.Peek();
            }
        }
    }

    (List<MazeCell>, Dictionary<MazeCell, int>) GetNeighbours(MazeCell currentCell)
    {
        List<MazeCell> neighboursList = new();
        Dictionary<MazeCell, int> neighboursDic = new();

        for (int x = 1, i = 1; x > -2; x -= 2, i += 2)
        {
            int neihbourX = currentCell.x + x;

            if(neihbourX < 0 || neihbourX >= cellsAmountX)
                continue;

            MazeCell neighbour = mazeGrid[neihbourX, currentCell.y];
            if(currentPath.Contains(neighbour) || completedPath.Contains(neighbour))
                continue;

            neighboursList.Add(neighbour);
            neighboursDic.Add(neighbour, i);
        }

        for (int y = 1, i = 0; y > -2; y -= 2, i += 2)
        {
            int neihbourY = currentCell.y + y;

            if(neihbourY < 0 || neihbourY >= cellsAmountY)
                continue;

            MazeCell neighbour = mazeGrid[currentCell.x, neihbourY];
            if(currentPath.Contains(neighbour) || completedPath.Contains(neighbour))
                continue;

            neighboursList.Add(neighbour);
            neighboursDic.Add(neighbour, i);
        }
        return (neighboursList, neighboursDic);
    }

    void RemoveWalls(MazeCell cell, int wallDir) => cell.GetWall(wallDir).SetActive(false);
    
    // void OnDrawGizmos()
    // {
    //     foreach (MazeCell cell in mazeGrid)
    //     {
    //         Gizmos.color = cell == currentCell ? Color.red : completedPath.Contains(cell) ? Color.blue : currentPath.Contains(cell) ? Color.yellow : Color.white;
    //         Gizmos.DrawCube(cell.transform.position, new(.5f, .5f, .5f));
    //     }
    // }
}
