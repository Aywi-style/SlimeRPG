using Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/Enemy")]
public class EnemyConfig : ScriptableObject
{
    [field: SerializeField] public EnemyMB EnemyMB { private set; get; }
    [field: SerializeField] public float Speed { private set; get; }
    [field: SerializeField] public int StartHealth { private set; get; }
    [field: SerializeField] public int StartDamage { private set; get; }
    [field: SerializeField] public float StartAttackSpeed { private set; get; }
    [field: SerializeField] public float DetectionDistance { private set; get; }
    [field: SerializeField] public int MaxEnemySpawn { private set; get; }
    [field: SerializeField] public int MaxSpawnOffset { private set; get; }
    [field: SerializeField] public int RewardValue { private set; get; }
    
}
