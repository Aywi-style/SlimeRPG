using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMB : MonoBehaviour
{
    [field: SerializeField] public Transform FirePoint { private set; get; }
    [field: SerializeField] public Transform PointForHealthbar { private set; get; }
}
