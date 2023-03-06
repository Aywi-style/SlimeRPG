using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Client;

public class ResourcesCanvasMB : MonoBehaviour
{
    [SerializeField] private Text _monyeCountText;
    [SerializeField] private Text _killCountText;

    private GameState _gameState;

    private void Start()
    {
        _gameState = GameState.Get();
        GameState.OnMoneyValueChanged += SetMoneyCount;
        GameState.OnKillCountChanged += SetKillCount;
    }

    private void OnDestroy()
    {
        GameState.OnMoneyValueChanged -= SetMoneyCount;
        GameState.OnKillCountChanged -= SetKillCount;
    }

    private void SetMoneyCount(int value)
    {
        _monyeCountText.text = value.ToString();
    }

    private void SetKillCount(int value)
    {
        _killCountText.text = value.ToString();
    }
}
