using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static void LoadScene(Scenes scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }
}

public enum Scenes
{
    MainMenu,
    Maze
}
