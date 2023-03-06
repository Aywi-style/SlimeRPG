using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class ProjectileFlyingSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;

        readonly EcsFilterInject<Inc<Projectile, Damagable, View>> _projectileFilter = default;

        readonly EcsPoolInject<View> _viewPool = default;
        readonly EcsPoolInject<Damagable> _damagablePool = default;
        readonly EcsPoolInject<Projectile> _projectilePool = default;

        readonly EcsPoolInject<DamagingEvent> _damagingEventPool = default;
        public void Run (IEcsSystems systems)
        {
            foreach (var projectileEntity in _projectileFilter.Value)
            {
                ref var view = ref _viewPool.Value.Get(projectileEntity);
                ref var projectile = ref _projectilePool.Value.Get(projectileEntity);
                ref var damagable = ref _damagablePool.Value.Get(projectileEntity);

                float distanceOverall = Vector3.Distance(projectile.StartPosition, projectile.TargetTransform.position);

                float distanceCovered = Vector3.Distance(projectile.StartPosition, view.GameObject.transform.position);

                float distanceLeft = Vector3.Distance(view.GameObject.transform.position, projectile.TargetTransform.position);

                if ((distanceOverall > distanceCovered + distanceLeft) || (distanceOverall < Mathf.Abs(distanceCovered - distanceLeft)))
                {
                    // Здесь могла быть ваша ошибка
                    Debug.LogError("Произошёл прикек с рассчетом координат для летящего снаряда");
                    continue;
                }

                Vector3 relativePos = (projectile.SupportPosition - view.Transform.position);
                Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                view.Transform.rotation = rotation;

                float speed;

                if (view.GameObject.transform.position.y > projectile.SupportPosition.y)
                {
                    speed = projectile.Speed * projectile.SpeedDecreaseFactor * Time.deltaTime;
                }
                else
                {
                    speed = projectile.Speed * projectile.SpeedIncreaseFactor * Time.deltaTime;
                }

                projectile.SupportPosition = Vector3.MoveTowards(projectile.SupportPosition,
                projectile.TargetTransform.position, speed);
                view.GameObject.transform.position = Vector3.MoveTowards(view.GameObject.transform.position,
                projectile.SupportPosition, speed);

                if (view.GameObject.transform.position == projectile.TargetTransform.position)
                {
                    var damagingEventEntity = _world.Value.NewEntity();
                    ref var damagingEventComponent = ref _damagingEventPool.Value.Add(damagingEventEntity);
                    damagingEventComponent.TargetEntity = projectile.TargetEntity;
                    damagingEventComponent.DamageValue = damagable.Value;

                    view.GameObject.SetActive(false);

                    _world.Value.DelEntity(projectileEntity);
                }
            }
        }
    }
}