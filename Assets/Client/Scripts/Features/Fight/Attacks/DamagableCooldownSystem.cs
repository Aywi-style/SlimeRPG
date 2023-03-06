using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class DamagableCooldownSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<DamagableCooldown>> _damagableCooldownFilter = default;

        readonly EcsPoolInject<DamagableCooldown> _damagableCooldownPool = default;

        public void Run (IEcsSystems systems)
        {
            foreach (var entity in _damagableCooldownFilter.Value)
            {
                ref var damagableCooldown = ref _damagableCooldownPool.Value.Get(entity);

                damagableCooldown.CurrentValue -= Time.deltaTime;

                if (damagableCooldown.CurrentValue <= 0)
                {
                    _damagableCooldownPool.Value.Del(entity);
                }
            }
        }
    }
}