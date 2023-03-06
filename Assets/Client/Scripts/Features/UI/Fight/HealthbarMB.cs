using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarMB : MonoBehaviour
{
    [field: SerializeField] public RectTransform Self { private set; get; }
    [field: SerializeField] public RectTransform Health { private set; get; }

    public void UpdateBar(float maxValue, float currentValue)
    {
        var healthPercent = currentValue / maxValue;
        var healthAnchorMax = Health.anchorMax;
        healthAnchorMax.x = healthPercent;
        Health.anchorMax = healthAnchorMax;
    }
}
