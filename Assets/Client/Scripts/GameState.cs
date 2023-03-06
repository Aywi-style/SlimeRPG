using UnityEngine;
using Client;
using System.Collections.Generic;
using System;
using Leopotam.EcsLite;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameState
{
    private static GameState _gameState = null;

    public EcsWorld EcsWorld;

    public AllPools AllPools;
    public AllPools ActivePools;

    public ChunksConfig ChunksConfig { private set; get; }
    public PlayerConfig PlayerConfig { private set; get; }
    public EnemyConfig EnemyConfig { private set; get; }
    public UiConfig UiConfig { private set; get; }
    public UpgradesConfig UpgradesConfig { private set; get; }

    public Queue<int> AllEnabledChunks;
    public int LastEnabledChunkEntity;

    public int PlayerKillCount { private set; get; }
    public int PlayerMoneyCount { private set; get; }
    public static Action<int> OnKillCountChanged;
    public static Action<int> OnMoneyValueChanged;
    public static Action OnPlayerKilled;

    #region Entities
    public const int NULL_ENTITY = -1;
    public int PlayerEntity { set; get; }
    #endregion

    #region Audio
    [field:SerializeField]
    public AudioMixer AudioPack { private set; get; }
    [field: SerializeField]
    public AmbientType AmbientType { private set; get; }

    public bool IsNeedChangeEmbient = false;
    #endregion
    
    private GameState(in EcsStartup ecsStartup)
    {
        EcsWorld = ecsStartup.World;

        AllPools = ecsStartup.AllPools;

        AudioPack = ecsStartup.AudioPack;
        AmbientType = ecsStartup.AmbientType;

        ChunksConfig = ecsStartup.ChunksConfig;
        PlayerConfig = ecsStartup.PlayerConfig;
        EnemyConfig = ecsStartup.EnemyConfig;
        UiConfig = ecsStartup.UiConfig;
        UpgradesConfig = ecsStartup.UpgradesConfig;
    }

    public static GameState InitializeNew(in EcsStartup ecsStartup)
    {
        _gameState = new GameState(in ecsStartup);

        return _gameState;
    }

    public static GameState Get()
    {
        return _gameState;
    }

    public void MoneyValueChange(int value)
    {
        if (PlayerMoneyCount < -value)
        {
            value = -PlayerMoneyCount;
        }

        PlayerMoneyCount += value;

        OnMoneyValueChanged?.Invoke(PlayerMoneyCount);
    }

    public void IncreaseKillCount()
    {
        PlayerKillCount++;

        OnKillCountChanged?.Invoke(PlayerKillCount);
    }

    public void UpgradePlayer(UpgradeType upgradeType, float value)
    {
        ref var upgradeEvent = ref EcsWorld.GetPool<UpgradeEvent>().Add(EcsWorld.NewEntity());
        upgradeEvent.UpgradeType = upgradeType;
        upgradeEvent.Value = value;
    }

    public void KillPlayer()
    {
        OnPlayerKilled?.Invoke();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
