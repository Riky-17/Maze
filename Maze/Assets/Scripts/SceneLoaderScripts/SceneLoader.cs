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
    Scenes sceneToLoad;
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

    public void LoadScene(Scenes scene)
    {
        sceneToLoad = scene;
        StartCoroutine(LoadSceneAsync(scene));
    }

    IEnumerator LoadSceneAsync(Scenes scene)
    {
        loadingScreenCanvas.SetActive(true);
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(scene.ToString());

        while (!loadingOperation.isDone)
        {
            loadingBar.fillAmount = loadingOperation.progress;
            Debug.Log(loadingOperation.progress);
            Debug.Log(loadingBar.fillAmount);
            yield return null;
        }
        loadingScreenCanvas.SetActive(false);
        CurrentScene = sceneToLoad;
    }
}

public enum Scenes
{
    MainMenu,
    Maze
}
