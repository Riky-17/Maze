using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    public static event Action<bool> onPauseToggle;
    public static event Action<float> onTimeChange;

    public Difficulties difficulty {get; private set;}
    public int MazeWidth {get; private set;}
    public int MazeHeight {get; private set;}

    GameStates state = GameStates.MainMenu;

    bool isPaused = false;

    public float ElapsedTime {get; private set;}
    int prevSecond;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        Time.timeScale = 1;
    }

    void OnEnable()
    {
        MazeGenerator.onPlayerInside += SetPlayerInside;
        MazeGenerator.onWin += SetStateWin;
    }

    void OnDisable()
    {
        MazeGenerator.onPlayerInside -= SetPlayerInside;
        MazeGenerator.onWin -= SetStateWin;
    }

    void Update()
    {
        if(SceneLoader.Instance.CurrentScene == Scenes.MainMenu)
            return;

        if(Input.GetKeyDown(KeyCode.Escape) && (state == GameStates.PlayerInside || state == GameStates.PlayerOutside))
            TogglePause();
        
        if(!isPaused && state == GameStates.PlayerInside)
        {
            ElapsedTime += Time.deltaTime;
            int seconds = Mathf.FloorToInt(ElapsedTime % 60);
            if(seconds != prevSecond)
            {
                prevSecond = seconds;
                onTimeChange?.Invoke(ElapsedTime);
            }
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        onPauseToggle?.Invoke(isPaused);
        if(isPaused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void SetDifficulty(Difficulties difficulty)
    {
        this.difficulty = difficulty;
        state = GameStates.PlayerOutside;
        if(difficulty == Difficulties.Custom)
            return;
        SetMazeSize();
    }

    public void SetCustomSize(int mazeWidth, int mazeHeight)
    {
        MazeWidth = mazeWidth;
        MazeHeight = mazeHeight;
    }

    void SetMazeSize()
    {
        switch (difficulty)
        {
            case Difficulties.Easy:
            MazeHeight = MazeWidth = 15;
            break;
            case Difficulties.Medium:
            MazeHeight = MazeWidth = 30;
            break;
            case Difficulties.Hard:
            MazeHeight = MazeWidth = 50;
            break;
        }
    }

    void SetPlayerInside() => state = GameStates.PlayerInside;
    void SetStateWin() => state = GameStates.Win;
}

public enum GameStates
{
    MainMenu,
    PlayerOutside,
    PlayerInside,
    Win
}

public enum Difficulties
{
    Easy,
    Medium,
    Hard,
    Custom
}
