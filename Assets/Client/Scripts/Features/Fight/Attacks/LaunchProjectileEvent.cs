using UnityEngine;

namespace Client
{
    struct LaunchProjectileEvent
    {
        public Transform FirePoint;
        public Transform PointForFiring;
        public int Damage;
        public float Speed;
        public int TargetEntity;
    }
}