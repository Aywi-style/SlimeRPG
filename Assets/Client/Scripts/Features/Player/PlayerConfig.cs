using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/Player")]
public class PlayerConfig : ScriptableObject
{
    [field: SerializeField] public PlayerMB PlayerMB { private set; get; }
    [field: SerializeField] public GameObject ProjectilePrefab { private set; get; }
    [field: SerializeField] public float Speed { private set; get; }
    [field: SerializeField] public int StartHealth { private set; get; }
    [field: SerializeField] public int StartDamage { private set; get; }
    [field: SerializeField] public float StartAttackSpeed { private set; get; }
    [field: SerializeField] public float StartHealthRecovery { private set; get; }

    [field: Space]
    [field: SerializeField] public float DetectingDistance { private set; get; }
    [field: SerializeField] public float ProjectileSpeed { private set; get; }
}
