using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "UiConfig", menuName = "Configs/UI")]
public class UiConfig : ScriptableObject
{
    [field: SerializeField] public MainUiPrefabMB MainUiPrefabMB { private set; get; }
    [field: SerializeField] public HealthbarMB HealthbarMBPrefab { private set; get; }
    [field: SerializeField] public UIDamageNumberMB UIDamageNumberMB { private set; get; }
    [field: SerializeField] public float UIDamageNumberLifeTime { private set; get; }
}
