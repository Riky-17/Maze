using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public static MazeGenerator Instance {get; private set;}

    [SerializeField] MazeCell[] mazeCellPrefabs;
    [SerializeField] Transform exitHall;
    [SerializeField] LayerMask playerMask;
    Vector2 Offset => new(0, mazeHeight + 5 + 7.5f);
    int mazeWidth;
    int mazeHeight;
    int MazeCellRadius => 1;
    int MazeCellDiameter => MazeCellRadius * 2;
    MazeCell[,] mazeGrid;
    
    Stack<MazeCell> currentPath;
    List<MazeCell> completedPath;

    public bool IsPlayerInside {get; private set;} = false;

    void Awake()
    {
        Instance = this;

        mazeWidth = GameManager.Instance.MazeWidth;
        mazeHeight = GameManager.Instance.MazeHeight;
        mazeGrid = new MazeCell[mazeWidth, mazeHeight];
        currentPath = new(mazeWidth * mazeHeight);
        completedPath = new(mazeWidth * mazeHeight);
    }

    void Start()
    {
        CreateGrid();
        GenerateMaze();
    }

    void FixedUpdate()
    {
        if(!IsPlayerInside)
            CheckPlayerInside();
    }

    void CreateGrid()
    {
        Vector2 bottomLeftCorner = new(-mazeWidth, -mazeHeight);

        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                float xPos = x * MazeCellDiameter + MazeCellRadius;
                float yPos = y * MazeCellDiameter + MazeCellRadius;
                Vector2 cellPos = new Vector2(xPos, yPos) + bottomLeftCorner + Offset;

                int mazeCellPrefabIndex = x == 0 || x == mazeWidth - 1 || y == 0 || y == mazeHeight - 1 ? 0 : 1;
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
        int exitPosX = Random.Range(0, mazeWidth);
        currentCell = mazeGrid[exitPosX, mazeHeight - 1];
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
        Collider2D collider = Physics2D.OverlapBox((Vector2)transform.position + Offset, new(mazeWidth * 2, mazeHeight * 2 - 3), 0, playerMask); // the -3 is to prevent the player from locking himself outside the maze
        if(collider == null)
            return;

        MazeCell entranceCell;
        int entranceCellx = mazeWidth / 2;
        // check if there are 2 entrance cells or 1
        if (mazeWidth % 2 == 0)
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
        IsPlayerInside = true;
    }

    void FixEdgeWalls(MazeCell cell)
    {
        if((cell.x == 0 || cell.x == mazeWidth - 1) && (cell.y == 0 || cell.y == mazeHeight - 1))
            return;

        List<Transform> cellWalls = cell.GetWalls();

        if(cell.x == 0)
            cellWalls[1].localScale += new Vector3(0, .2f, 0);
        else if(cell.x == mazeWidth - 1)
            cellWalls[3].localScale += new Vector3(0, .2f, 0);

        if(cell.y == 0)
            cellWalls[0].localScale += new Vector3(.2f, 0, 0);
        else if(cell.y == mazeHeight - 1)
            cellWalls[2].localScale += new Vector3(.2f, 0, 0);
    }

    void RemoveEntranceWalls()
    {
        MazeCell entranceCell;
        int entranceCellx = mazeWidth / 2;
        // check if there are 2 entrance cells or 1
        if (mazeWidth % 2 == 0)
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

            if(neihbourX < 0 || neihbourX >= mazeWidth)
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

            if(neihbourY < 0 || neihbourY >= mazeHeight)
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
