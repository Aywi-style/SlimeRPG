using UnityEngine;

namespace Client
{
    struct Health
    {
        public int MaxValue;
        public int CurrentValue;
        public HealthbarMB HealthbarMB;
        public Transform PointForHealthbar;
    }
}