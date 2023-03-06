using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Data;
using UnityEngine;

namespace Client
{
    sealed class EnemyChooseTargetSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;

        readonly EcsFilterInject<Inc<Enemy, Targetable>, Exc<Dead>> _enemyFilter = default;

        readonly EcsPoolInject<DamagingEvent> _damagingEventPool = default;
        readonly EcsPoolInject<StartDamagableCooldownEvent> _startDamagableCooldownEventPool = default;

        readonly EcsPoolInject<Targetable> _targetablePool = default;
        readonly EcsPoolInject<View> _viewPool = default;
        readonly EcsPoolInject<Damagable> _damagablePool = default;

        public void Run (IEcsSystems systems)
        {
            foreach (var enemyEntity in _enemyFilter.Value)
            {
                ref var view = ref _viewPool.Value.Get(enemyEntity);
                ref var targetable = ref _targetablePool.Value.Get(enemyEntity);

                if (targetable.TargetedEnemy != GameState.NULL_ENTITY)
                {
                    continue;
                }

                ref var playerView = ref _viewPool.Value.Get(targetable.EnemyArray[0]);

                var distanceToPlayer = Vector3.Distance(view.Transform.position, playerView.Transform.position);

                if (distanceToPlayer > targetable.DetectingDistance)
                {
                    continue;
                }

                targetable.TargetedEnemy = targetable.EnemyArray[0];
            }
        }
    }
}