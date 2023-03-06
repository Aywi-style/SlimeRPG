using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiUpgradeMB : MonoBehaviour
{
    [field: SerializeField] public int Level;
    [field: SerializeField] public Text LevelText;
    [field: SerializeField] public float Value;
    [field: SerializeField] public Text ValueText;
    [field: SerializeField] public int Cost;
    [field: SerializeField] public Text CostText;

    [field: SerializeField] public Button EnhanceButton;
}
