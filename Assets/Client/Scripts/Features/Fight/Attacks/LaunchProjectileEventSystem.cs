using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class LaunchProjectileEventSystem : IEcsRunSystem
    {
        readonly EcsCustomInject<GameState> _gameState;

        readonly EcsWorldInject _world;

        readonly EcsFilterInject<Inc<LaunchProjectileEvent>> _eventFilter = default;

        readonly EcsPoolInject<LaunchProjectileEvent> _eventPool = default;

        readonly EcsPoolInject<Projectile> _projectilePool = default;
        readonly EcsPoolInject<View> _viewPool = default;
        readonly EcsPoolInject<Damagable> _damagablePool = default;

        private int _eventEntity = GameState.NULL_ENTITY;

        public void Run (IEcsSystems systems)
        {
            foreach (var eventEntity in _eventFilter.Value)
            {
                _eventEntity = eventEntity;

                ref var eventComponent = ref _eventPool.Value.Get(_eventEntity);

                var projectileEntity = _world.Value.NewEntity();

                ref var view = ref _viewPool.Value.Add(projectileEntity);
                view.GameObject = GameObject.Instantiate(_gameState.Value.PlayerConfig.ProjectilePrefab);
                view.Transform = view.GameObject.transform;
                view.Transform.position = eventComponent.FirePoint.position;

                ref var projectile = ref _projectilePool.Value.Add(projectileEntity);
                projectile.Speed = eventComponent.Speed;
                projectile.SpeedDecreaseFactor = 1.2f;
                projectile.SpeedIncreaseFactor = 0.8f;
                projectile.StartPosition = eventComponent.FirePoint.position;
                projectile.TargetEntity = eventComponent.TargetEntity;
                projectile.TargetTransform = eventComponent.PointForFiring;

                float startDistance = Vector3.Distance(eventComponent.FirePoint.position, eventComponent.PointForFiring.position);
                Vector3 positionBetween = Vector3.Lerp(eventComponent.FirePoint.position, eventComponent.PointForFiring.position, 0.5f);
                Vector3 supportPosition = new Vector3(positionBetween.x, eventComponent.FirePoint.position.y + startDistance * 0.5f, positionBetween.z);

                projectile.SupportPosition = supportPosition;

                ref var damagable = ref _damagablePool.Value.Add(projectileEntity);
                damagable.Value = eventComponent.Damage;

                DeleteEvent();
            }
        }

        private void DeleteEvent()
        {
            _eventPool.Value.Del(_eventEntity);

            _eventEntity = GameState.NULL_ENTITY;
        }
    }
}