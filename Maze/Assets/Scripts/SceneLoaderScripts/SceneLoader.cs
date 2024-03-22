using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    static Scenes sceneToLoad;
    public static Scenes CurrentScene {get; private set;} = Scenes.MainMenu;
    const string LOADING_SCREEN_SCENE = "LoadingScreen";

    public static void LoadScene(Scenes scene)
    {
        sceneToLoad = scene;
        SceneManager.LoadScene(LOADING_SCREEN_SCENE);
    }

    public static void LoadingScreenCallBack()
    {
        CurrentScene = sceneToLoad;
        SceneManager.LoadScene(sceneToLoad.ToString());
    }
}

public enum Scenes
{
    MainMenu,
    Maze
}
