using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Data;
using UnityEngine;

namespace Client
{
    sealed class EnemyAttackSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;

        readonly EcsFilterInject<Inc<Enemy, Targetable>, Exc<Dead, DamagableCooldown>> _enemyFilter = default;

        readonly EcsPoolInject<DamagingEvent> _damagingEventPool = default;
        readonly EcsPoolInject<StartDamagableCooldownEvent> _startDamagableCooldownEventPool = default;

        readonly EcsPoolInject<Targetable> _targetablePool = default;
        readonly EcsPoolInject<Damagable> _damagablePool = default;

        public void Run (IEcsSystems systems)
        {
            foreach (var enemyEntity in _enemyFilter.Value)
            {
                ref var targetable = ref _targetablePool.Value.Get(enemyEntity);

                if (targetable.TargetedEnemy == GameState.NULL_ENTITY)
                {
                    continue;
                }

                ref var damagable = ref _damagablePool.Value.Get(enemyEntity);

                var damagingEventEntity = _world.Value.NewEntity();
                ref var damagingEventComponent = ref _damagingEventPool.Value.Add(damagingEventEntity);
                damagingEventComponent.TargetEntity = targetable.TargetedEnemy;
                damagingEventComponent.DamageValue = damagable.Value;

                ref var startDamagableCooldownEvent = ref _startDamagableCooldownEventPool.Value.Add(_world.Value.NewEntity());
                startDamagableCooldownEvent.Entity = enemyEntity;
            }
        }
    }
}