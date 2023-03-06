using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System;
using UnityEngine;

namespace Client
{
    sealed class EnemySpawnSystem : IEcsRunSystem
    {
        readonly EcsCustomInject<GameState> _gameState;

        readonly EcsWorldInject _world;

        readonly EcsFilterInject<Inc<Enemy>, Exc<Dead>> _aliveEnemyFilter = default;
        readonly EcsFilterInject<Inc<Player>, Exc<Dead>> _alivePlayerFilter = default;

        readonly EcsPoolInject<EnemySpawnEvent> _enemySpawnEventPool = default;

        readonly EcsPoolInject<View> _viewPool = default;

        private Quaternion _enemySpawnRoration = new Quaternion(0, 180f, 0, 0);

        private int _standartEnemyCount = 1;

        private Vector3 _spawnPoint = Vector3.zero;
        private float _spawnOffsetX = 0.7f;
        private float _spawnOffsetZ = 2f;

        private int _spawnCount = 0;

        public void Run (IEcsSystems systems)
        {
            if (_aliveEnemyFilter.Value.GetEntitiesCount() > 0)
            {
                return;
            }

            if (_alivePlayerFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }

            var squareRoot = Mathf.FloorToInt(Mathf.Sqrt(_gameState.Value.PlayerKillCount));

            var maxExtraEnemy = squareRoot;

            if (squareRoot > _gameState.Value.EnemyConfig.MaxEnemySpawn)
            {
                maxExtraEnemy = _gameState.Value.EnemyConfig.MaxEnemySpawn;
            }

            var extraEnemy = UnityEngine.Random.Range(0, maxExtraEnemy);
            var enemyToSpawn = _standartEnemyCount + extraEnemy;

            var halfSpawnOffsetX = _spawnOffsetX / 2;
            var startPointX = (enemyToSpawn - 1) * (-halfSpawnOffsetX);

            ref var playerView = ref _viewPool.Value.Get(_gameState.Value.PlayerEntity);

            for (int i = 0; i < enemyToSpawn; i++)
            {
                _spawnPoint.x = startPointX + (i * _spawnOffsetX);
                _spawnPoint.z = playerView.Transform.position.z + _gameState.Value.EnemyConfig.MaxSpawnOffset + UnityEngine.Random.Range(0, _spawnOffsetZ);

                ref var enemySpawn = ref _enemySpawnEventPool.Value.Add(_world.Value.NewEntity());
                enemySpawn.SpawnPoint = _spawnPoint;
                enemySpawn.Rotation = _enemySpawnRoration;

                _spawnCount++;
            }
        }
    }
}