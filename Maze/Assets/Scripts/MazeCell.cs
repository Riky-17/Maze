using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    //the walls are set in a clockwise order starting from the top wall
    [SerializeField] List<Transform> walls;

    [HideInInspector] public int x;
    [HideInInspector] public int y;

    public GameObject GetWall(int i) => walls[i].gameObject;
}
