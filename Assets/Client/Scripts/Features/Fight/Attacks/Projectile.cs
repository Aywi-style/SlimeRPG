using UnityEngine;

namespace Client
{
    struct Projectile
    {
        public float Speed;
        public float SpeedDecreaseFactor;
        public float SpeedIncreaseFactor;
        public Vector3 StartPosition;
        public Vector3 SupportPosition;
        public Transform TargetTransform;
        public int TargetEntity;
    }
}