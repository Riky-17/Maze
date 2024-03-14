using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    public Difficulties difficulty {get; private set;}
    public float MazeWidth {get; private set;}
    public float MazeHeight {get; private set;}

    public static event Action<bool> onPauseToggle;
    bool isPaused = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        Time.timeScale = 1;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
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
        if(difficulty == Difficulties.Custom)
            return;
        SetMazeSize();
    }

    void SetMazeSize()
    {
        switch (difficulty)
        {
            case Difficulties.Easy:
            MazeHeight = MazeWidth = 20;
            break;
            case Difficulties.Medium:
            MazeHeight = MazeWidth = 50;
            break;
            case Difficulties.Hard:
            MazeHeight = MazeWidth = 100;
            break;
        }
    }
}

public enum Difficulties
{
    Easy,
    Medium,
    Hard,
    Custom
}
