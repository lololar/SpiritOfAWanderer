using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

    public static Vector3 _gravity = new Vector3(0.0f, -0.91f, 0.0f);

    public enum GameState
    {
        BEGIN,
        GAME,
        END,
    }

    public GameState _state;

    void Update()
    {
        switch (_state)
        {
            case GameState.BEGIN:
                if (Input.GetButtonDown("Load"))
                {
                    PlayerManager.GetInstance.Play();
                }
                break;
            case GameState.GAME:
                break;
            case GameState.END:
                if (Input.GetButtonDown("Reload"))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                break;
            default:
                break;
        }
    }

    public void EndGame()
    {
        _state = GameState.END;
        PlayerManager.GetInstance.End();
    }

    public void BeginGame()
    {
        _state = GameState.GAME;
    }
}
