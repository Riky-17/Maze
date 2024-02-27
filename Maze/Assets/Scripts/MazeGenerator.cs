using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] MazeCell mazeCellPrefab;
    [SerializeField] float mazeWidth;
    [SerializeField] float mazeHeight;
    int cellsAmountX => Mathf.FloorToInt(mazeWidth) / mazeCellDiameter;
    int cellsAmountY => Mathf.FloorToInt(mazeHeight) / mazeCellDiameter;
    int mazeCellRadius = 1;
    int mazeCellDiameter => mazeCellRadius * 2;
    MazeCell[,] mazeGrid;
    Stack<MazeCell> currentPath = new();
    List<MazeCell> completedPath = new();
    MazeCell currentCell;

    void Awake()
    {
        mazeGrid = new MazeCell[cellsAmountX, cellsAmountY];
    }

    void Start()
    {
        CreateGrid();
        StartCoroutine(GenerateMaze());
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
                Vector2 cellPos = new Vector2(xPos, yPos) + bottomLeftCorner;

                MazeCell newCell = Instantiate(mazeCellPrefab, cellPos, Quaternion.identity);
                (newCell.x, newCell.y) = (x, y);
                mazeGrid[x, y] = newCell;
            }
        }
    }

    IEnumerator GenerateMaze()
    {
        currentCell = mazeGrid[5, 9];
        currentPath.Push(currentCell);
        List<MazeCell> neighboursList;
        Dictionary<MazeCell, int> neighboursDic;

        //int forceDone = 0;
        while(currentPath.Count > 0)
        {
            //forceDone++;
            //if(forceDone >= 100) 
                //break;
            //previousCell = currentPath.Peek();
            (neighboursList, neighboursDic) = GetNeighbours(currentCell);
            //Debug.Log(neighboursList.Count > 0);
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
            yield return null;
        }
    }

    void RemoveWalls(MazeCell cell, int wallDir)
    {
        cell.GetWall(wallDir).SetActive(false);
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

            if(neihbourY < 0 || neihbourY >= cellsAmountX)
                continue;

            MazeCell neighbour = mazeGrid[currentCell.x, neihbourY];
            if(currentPath.Contains(neighbour) || completedPath.Contains(neighbour))
                continue;

            neighboursList.Add(neighbour);
            neighboursDic.Add(neighbour, i);
        }
        return (neighboursList, neighboursDic);
    }

    void OnDrawGizmos()
    {
        foreach (MazeCell cell in mazeGrid)
        {
            Gizmos.color = cell == currentCell ? Color.red : completedPath.Contains(cell) ? Color.blue : currentPath.Contains(cell) ? Color.yellow : Color.white;
            Gizmos.DrawCube(cell.transform.position, new(.5f, .5f, .5f));
        }
    }
}
