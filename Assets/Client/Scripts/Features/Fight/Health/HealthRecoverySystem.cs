using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class HealthRecoverySystem : IEcsRunSystem
    {
        readonly EcsCustomInject<GameState> _gameState;

        readonly EcsFilterInject<Inc<Health, HealthRecovery>> _healthRecoveryFilter = default;

        readonly EcsPoolInject<Health> _healthPool = default;
        readonly EcsPoolInject<HealthRecovery> _healthRecoveryPool = default;

        public void Run (IEcsSystems systems)
        {
            foreach (var recoveryEntity in _healthRecoveryFilter.Value)
            {
                ref var health = ref _healthPool.Value.Get(recoveryEntity);

                if (health.CurrentValue >= health.MaxValue)
                {
                    continue;
                }

                ref var healthRecovery = ref _healthRecoveryPool.Value.Get(recoveryEntity);
                healthRecovery.Buffer += healthRecovery.Value * Time.deltaTime;

                if (healthRecovery.Buffer < 1)
                {
                    continue;
                }

                var healingValue = Mathf.FloorToInt(healthRecovery.Buffer);

                health.CurrentValue += healingValue;
                healthRecovery.Buffer -= healingValue;

                if (health.CurrentValue > health.MaxValue)
                {
                    health.CurrentValue = health.MaxValue;
                }

                health.HealthbarMB?.UpdateBar(health.MaxValue, health.CurrentValue);
            }
        }
    }
}