using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiedCanvasMB : MonoBehaviour
{
    private GameState _gameState;

    [SerializeField] private Canvas _self;

    private void Start()
    {
        _gameState = GameState.Get();

        GameState.OnPlayerKilled += EnableCanvas;
    }

    private void OnDestroy()
    {
        GameState.OnPlayerKilled -= EnableCanvas;
    }

    public void Restart()
    {
        _gameState.RestartLevel();
    }

    private void EnableCanvas()
    {
        _self.enabled = true;
    }
}
