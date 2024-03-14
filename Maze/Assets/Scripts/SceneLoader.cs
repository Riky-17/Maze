using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    static Scenes sceneToLoad;
    const string LOADING_SCREEN_SCENE = "LoadingScreen";

    public static void LoadScene(Scenes scene)
    {
        sceneToLoad = scene;
        SceneManager.LoadScene(LOADING_SCREEN_SCENE);
    }

    public static void LoadingScreenCallBack() => SceneManager.LoadScene(sceneToLoad.ToString());
}

public enum Scenes
{
    MainMenu,
    Maze
}
