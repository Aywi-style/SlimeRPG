using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class StandingSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<Targetable>> _targetableFilter = default;

        readonly EcsPoolInject<Targetable> _targetablePool = default;
        readonly EcsPoolInject<Standing> _standingPool = default;

        public void Run (IEcsSystems systems)
        {
            foreach (var entity in _targetableFilter.Value)
            {
                ref var targetable = ref _targetablePool.Value.Get(entity);

                if (targetable.TargetedEnemy == GameState.NULL_ENTITY)
                {
                    if (_standingPool.Value.Has(entity))
                    {
                        _standingPool.Value.Del(entity);
                    }
                }
                else
                {
                    if (!_standingPool.Value.Has(entity))
                    {
                        _standingPool.Value.Add(entity);
                    }
                }

            }
        }
    }
}