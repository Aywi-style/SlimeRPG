using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class EnemySpawnEventSystem : IEcsRunSystem
    {
        readonly EcsCustomInject<GameState> _gameState;

        readonly EcsWorldInject _world;

        readonly EcsFilterInject<Inc<EnemySpawnEvent>> _eventFilter = default;

        readonly EcsPoolInject<EnemySpawnEvent> _eventPool = default;

        readonly EcsPoolInject<View> _viewPool = default;
        readonly EcsPoolInject<Enemy> _enemyPool = default;
        readonly EcsPoolInject<Movable> _movablePool = default;
        readonly EcsPoolInject<Damagable> _damagablePool = default;
        readonly EcsPoolInject<Health> _healthPool = default;
        readonly EcsPoolInject<Targetable> _targetablePool = default;
        readonly EcsPoolInject<RewardGiver> _rewardGiverPool = default;

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

                ref var eventComponent = ref _eventPool.Value.Get(_eventEntity);

                var enemyEntity = _world.Value.NewEntity();

                ref var enemy = ref _enemyPool.Value.Add(enemyEntity);
                enemy.EnemyMB = GameObject.Instantiate(_gameState.Value.EnemyConfig.EnemyMB, eventComponent.SpawnPoint, eventComponent.Rotation);

                ref var view = ref _viewPool.Value.Add(enemyEntity);
                view.GameObject = enemy.EnemyMB.gameObject;
                view.Transform = enemy.EnemyMB.transform;

                ref var movable = ref _movablePool.Value.Add(enemyEntity);
                movable.Direction = Vector3.forward;
                movable.Speed = _gameState.Value.EnemyConfig.Speed;

                ref var damagable = ref _damagablePool.Value.Add(enemyEntity);
                damagable.Value = _gameState.Value.EnemyConfig.StartDamage;
                damagable.AttackSpeed = _gameState.Value.EnemyConfig.StartAttackSpeed;

                ref var targetable = ref _targetablePool.Value.Add(enemyEntity);
                targetable.DetectingDistance = _gameState.Value.EnemyConfig.DetectionDistance;
                targetable.EnemyArray = new int[1];
                targetable.EnemyArray[0] = _gameState.Value.PlayerEntity;
                targetable.TargetedEnemy = GameState.NULL_ENTITY;

                ref var health = ref _healthPool.Value.Add(enemyEntity);
                health.MaxValue = _gameState.Value.EnemyConfig.StartHealth;
                health.CurrentValue = health.MaxValue;
                health.PointForHealthbar = enemy.EnemyMB.PointForHealthbar;

                ref var rewardGiver = ref _rewardGiverPool.Value.Add(enemyEntity);
                rewardGiver.Value = _gameState.Value.EnemyConfig.RewardValue;

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