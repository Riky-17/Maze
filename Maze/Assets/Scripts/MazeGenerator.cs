using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] MazeCell[] mazeCellPrefabs;
    [SerializeField] Transform exitHall;
    [SerializeField] LayerMask playerMask;
    Vector2 Offset => new(0, RoundedHeight / 2 + 5 + 7.5f);
    [SerializeField] float mazeWidth;
    int RoundedWidth => Mathf.FloorToInt(mazeWidth);
    [SerializeField] float mazeHeight;
    int RoundedHeight => Mathf.FloorToInt(mazeHeight);
    int CellsAmountX => RoundedWidth / MazeCellDiameter;
    int CellsAmountY => RoundedHeight / MazeCellDiameter;
    int mazeCellRadius = 1;
    int MazeCellDiameter => mazeCellRadius * 2;
    MazeCell[,] mazeGrid;
    
    Stack<MazeCell> currentPath;
    List<MazeCell> completedPath;

    bool isPlayerInside = false;

    void Awake()
    {
        mazeGrid = new MazeCell[CellsAmountX, CellsAmountY];
        currentPath = new(CellsAmountX * CellsAmountY);
        completedPath = new(CellsAmountX * CellsAmountY);
    }

    void Start()
    {
        CreateGrid();
        GenerateMaze();
    }

    void FixedUpdate()
    {
        if(!isPlayerInside)
            CheckPlayerInside();
    }

    void CreateGrid()
    {
        Vector2 bottomLeftCorner = new(-CellsAmountX, -CellsAmountY);

        for (int x = 0; x < CellsAmountX; x++)
        {
            for (int y = 0; y < CellsAmountY; y++)
            {
                float xPos = x * MazeCellDiameter + mazeCellRadius;
                float yPos = y * MazeCellDiameter + mazeCellRadius;
                Vector2 cellPos = new Vector2(xPos, yPos) + bottomLeftCorner + Offset;

                int mazeCellPrefabIndex = x == 0 || x == CellsAmountX - 1 || y == 0 || y == CellsAmountY - 1 ? 0 : 1;
                MazeCell newCell = Instantiate(mazeCellPrefabs[mazeCellPrefabIndex], cellPos, Quaternion.identity);
                (newCell.x, newCell.y) = (x, y);
                if(mazeCellPrefabIndex == 0)
                    FixEdgeWalls(newCell);
                
                mazeGrid[x, y] = newCell;
            }
        }
    }

    void GenerateMaze()
    {
        RemoveEntranceWalls();

        MazeCell currentCell;
        int exitPosX = Random.Range(0, CellsAmountX);
        currentCell = mazeGrid[exitPosX, CellsAmountY - 1];
        exitHall.position = currentCell.transform.position + new Vector3(0, 5.8f, 0);
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
        RemoveWalls(currentCell, 0);
    }

    void CheckPlayerInside()
    {
        Collider2D collider = Physics2D.OverlapBox((Vector2)transform.position + Offset, new(RoundedWidth, RoundedHeight - 2), 0, playerMask);
        if(collider == null)
            return;

        MazeCell entranceCell;
        int entranceCellx = CellsAmountX / 2;
        // check if there are 2 entrance cells or 1
        if (CellsAmountX % 2 == 0)
        {
            entranceCell = mazeGrid[entranceCellx - 1, 0];
            PlaceWalls(entranceCell, 2);
            entranceCell = mazeGrid[entranceCellx, 0];
            PlaceWalls(entranceCell, 2);
        }
        else
        {
            entranceCell = mazeGrid[entranceCellx, 0];
            PlaceWalls(entranceCell, 2);
        }
        isPlayerInside = true;
    }

    void FixEdgeWalls(MazeCell cell)
    {
        if((cell.x == 0 || cell.x == CellsAmountX - 1) && (cell.y == 0 || cell.y == CellsAmountY - 1))
            return;

        List<Transform> cellWalls = cell.GetWalls();

        if(cell.x == 0)
            cellWalls[1].localScale += new Vector3(0, .2f, 0);
        else if(cell.x == CellsAmountX - 1)
            cellWalls[3].localScale += new Vector3(0, .2f, 0);

        if(cell.y == 0)
            cellWalls[0].localScale += new Vector3(.2f, 0, 0);
        else if(cell.y == CellsAmountY - 1)
            cellWalls[2].localScale += new Vector3(.2f, 0, 0);
    }

    void RemoveEntranceWalls()
    {
        MazeCell entranceCell;
        int entranceCellx = CellsAmountX / 2;
        // check if there are 2 entrance cells or 1
        if (CellsAmountX % 2 == 0)
        {
            entranceCell = mazeGrid[entranceCellx - 1, 0];
            RemoveWalls(entranceCell, 2);
            RemoveWalls(entranceCell, 1);
            entranceCell = mazeGrid[entranceCellx, 0];
            RemoveWalls(entranceCell, 2);
            RemoveWalls(entranceCell, 3);
        }
        else
        {
            entranceCell = mazeGrid[entranceCellx, 0];
            RemoveWalls(entranceCell, 2);
        }
    }

    (List<MazeCell>, Dictionary<MazeCell, int>) GetNeighbours(MazeCell currentCell)
    {
        List<MazeCell> neighboursList = new();
        Dictionary<MazeCell, int> neighboursDic = new();

        for (int x = 1, i = 1; x > -2; x -= 2, i += 2)
        {
            int neihbourX = currentCell.x + x;

            if(neihbourX < 0 || neihbourX >= CellsAmountX)
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

            if(neihbourY < 0 || neihbourY >= CellsAmountY)
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
    void PlaceWalls(MazeCell cell, int wallDir) => cell.GetWall(wallDir).SetActive(true);
    
    // void OnDrawGizmos()
    // {
    //     foreach (MazeCell cell in mazeGrid)
    //     {
    //         Gizmos.color = cell == currentCell ? Color.red 
    //                              : completedPath.Contains(cell) ? Color.blue 
    //                              : currentPath.Contains(cell) ? Color.yellow 
    //                              : Color.white;
    //         Gizmos.DrawCube(cell.transform.position, new(.5f, .5f, .5f));
    //     }
    // }
}
