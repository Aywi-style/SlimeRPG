using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradesConfig", menuName = "Configs/Upgrades")]
public class UpgradesConfig : ScriptableObject
{
    [field: SerializeField] public int AttackCostPerLevel { private set; get; }
    [field: SerializeField] public int AttackPerLevel { private set; get; }
    [field: SerializeField] public int HealthCostPerLevel { private set; get; }
    [field: SerializeField] public int HealthPerLevel { private set; get; }
    [field: SerializeField] public int HealthRecoveryCostPerLevel { private set; get; }
    [field: SerializeField] public float HealthRecoveryPerLevel { private set; get; }
    [field: SerializeField] public int AttackSpeedCostPerLevel { private set; get; }
    [field: SerializeField] public float AttackSpeedPerLevel { private set; get; }
}
