using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class PlayerDetectingSystem : IEcsRunSystem
    {
        readonly EcsCustomInject<GameState> _gameState = default;

        readonly EcsWorldInject _world = default;

        readonly EcsFilterInject<Inc<Player, Targetable>> _playerFilter = default;
        readonly EcsFilterInject<Inc<Enemy, View>, Exc<Dead>> _enemiesFilter = default;

        readonly EcsPoolInject<Player> _playerPool = default;
        readonly EcsPoolInject<Targetable> _targetablePool = default;
        readonly EcsPoolInject<View> _viewPool = default;

        public void Run (IEcsSystems systems)
        {
            var enemiesCount = _enemiesFilter.Value.GetEntitiesCount();

            if (enemiesCount <= 0)
            {
                return;
            }

            foreach (int playerEntity in _playerFilter.Value)
            {
                ref var player = ref _playerPool.Value.Get(playerEntity);
                ref var view = ref _viewPool.Value.Get(playerEntity);
                ref var targetable = ref _targetablePool.Value.Get(playerEntity);

                var rawEnemyEntities = _enemiesFilter.Value.GetRawEntities();

                if (enemiesCount > targetable.EnemyArray.Length)
                {
                    enemiesCount = targetable.EnemyArray.Length;
                }

                var enemyCell = 0;

                for (int i = 0; i < enemiesCount; i++)
                {
                    var enemyEntity = rawEnemyEntities[i];

                    ref var enemyView = ref _viewPool.Value.Get(enemyEntity);

                    var distanceToEnemy = Vector3.Distance(view.Transform.position, enemyView.Transform.position);

                    if (distanceToEnemy > targetable.DetectingDistance)
                    {
                        continue;
                    }

                    targetable.EnemyArray[enemyCell] = enemyEntity;
                    enemyCell++;
                }
            }
        }
    }
}