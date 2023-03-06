using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class ClearPlayerTargetedEnemy : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<Player, Targetable>> _playerFilter = default;

        readonly EcsPoolInject<Targetable> _targetablePool = default;
        readonly EcsPoolInject<Dead> _deadPool = default;

        public void Run (IEcsSystems systems)
        {
            foreach (var playerEntity in _playerFilter.Value)
            {
                ref var targetable = ref _targetablePool.Value.Get(playerEntity);

                for (int i = 0; i < targetable.EnemyArray.Length; i++)
                {
                    if (targetable.EnemyArray[i] == GameState.NULL_ENTITY)
                    {
                        continue;
                    }

                    if (_deadPool.Value.Has(targetable.EnemyArray[i]))
                    {
                        targetable.EnemyArray[i] = GameState.NULL_ENTITY;
                    }
                }


            }
        }
    }
}