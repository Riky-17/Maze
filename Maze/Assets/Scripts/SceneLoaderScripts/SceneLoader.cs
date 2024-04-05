using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance {get; private set;}

    [SerializeField] GameObject loadingScreenCanvas;
    [SerializeField] Image loadingBar;

    public Scenes CurrentScene {get; private set;} = Scenes.MainMenu;

    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(loadingScreenCanvas);
    }

    IEnumerator LoadSceneAsync(Scenes scene)
    {
        loadingScreenCanvas.SetActive(true);
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(scene.ToString());

        loadingOperation.allowSceneActivation = scene != Scenes.Maze;

        while (!loadingOperation.isDone)
        {
            loadingBar.fillAmount = loadingOperation.progress;
            yield return null;
        }
        Debug.Log("done");
        // loadingScreenCanvas.SetActive(false);
        CurrentScene = scene;
    }

    public void LoadScene(Scenes scene) => StartCoroutine(LoadSceneAsync(scene));
}

public enum Scenes
{
    MainMenu,
    Maze
}
