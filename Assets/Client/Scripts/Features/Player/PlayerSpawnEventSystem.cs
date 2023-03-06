using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class PlayerSpawnEventSystem : IEcsRunSystem
    {
        readonly EcsCustomInject<GameState> _gameState;

        readonly EcsWorldInject _world;

        readonly EcsFilterInject<Inc<PlayerSpawnEvent>> _eventFilter = default;

        readonly EcsPoolInject<PlayerSpawnEvent> _eventPool = default;

        readonly EcsPoolInject<View> _viewPool = default;
        readonly EcsPoolInject<Player> _playerPool = default;
        readonly EcsPoolInject<Movable> _movablePool = default;
        readonly EcsPoolInject<Targetable> _targetablePool = default;
        readonly EcsPoolInject<Damagable> _damagablePool = default;
        readonly EcsPoolInject<Health> _healthPool = default;
        readonly EcsPoolInject<HealthRecovery> _healthRecoveryPool = default;

        private int _eventEntity = GameState.NULL_ENTITY;

        public void Run (IEcsSystems systems)
        {
            if (_eventFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }

            foreach (var eventEntity in _eventFilter.Value)
            {
                _eventEntity = eventEntity;

                var playerEntity = _world.Value.NewEntity();
                _gameState.Value.PlayerEntity = playerEntity;

                ref var player = ref _playerPool.Value.Add(playerEntity);
                player.PlayerMB = GameObject.Instantiate(_gameState.Value.PlayerConfig.PlayerMB);

                ref var view = ref _viewPool.Value.Add(playerEntity);
                view.GameObject = player.PlayerMB.gameObject;
                view.Transform = player.PlayerMB.transform;

                ref var movable = ref _movablePool.Value.Add(playerEntity);
                movable.Direction = Vector3.forward;
                movable.Speed = _gameState.Value.PlayerConfig.Speed;

                ref var targetable = ref _targetablePool.Value.Add(playerEntity);
                targetable.DetectingDistance = _gameState.Value.PlayerConfig.DetectingDistance;
                targetable.EnemyArray = new int[5];

                for (int i = 0; i < 5; i++)
                {
                    targetable.EnemyArray[i] = GameState.NULL_ENTITY;
                }

                targetable.TargetedEnemy = GameState.NULL_ENTITY;

                ref var damagable = ref _damagablePool.Value.Add(playerEntity);
                damagable.Value = _gameState.Value.PlayerConfig.StartDamage;
                damagable.AttackSpeed = _gameState.Value.PlayerConfig.StartAttackSpeed;

                ref var health = ref _healthPool.Value.Add(playerEntity);
                health.MaxValue = _gameState.Value.PlayerConfig.StartHealth;
                health.CurrentValue = health.MaxValue;
                health.PointForHealthbar = player.PlayerMB.PointForHealthbar;

                ref var healthRecovery = ref _healthRecoveryPool.Value.Add(playerEntity);
                healthRecovery.Value = _gameState.Value.PlayerConfig.StartHealthRecovery;

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