using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMB : MonoBehaviour
{
    [field: SerializeField] public Transform PointForFiring { private set; get; }
    [field: SerializeField] public Transform PointForHealthbar { private set; get; }
}
