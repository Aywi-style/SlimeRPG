using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class TargetingOnDetectedEnemy : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<Player, Targetable>> _playerFilter = default;

        readonly EcsPoolInject<Targetable> _targetablePool = default;
        readonly EcsPoolInject<Dead> _deadPool = default;

        public void Run (IEcsSystems systems)
        {
            foreach (var playerEntity in _playerFilter.Value)
            {
                ref var targetable = ref _targetablePool.Value.Get(playerEntity);

                if (targetable.TargetedEnemy != GameState.NULL_ENTITY)
                {
                    if (!_deadPool.Value.Has(targetable.TargetedEnemy))
                    {
                        continue;
                    }
                }

                targetable.TargetedEnemy = GameState.NULL_ENTITY;

                if (targetable.EnemyArray[0] == GameState.NULL_ENTITY)
                {
                    continue;
                }

                targetable.TargetedEnemy = targetable.EnemyArray[0];
            }
        }
    }
}