using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class HealthbarSystem : IEcsRunSystem
    {
        readonly EcsCustomInject<GameState> _gameState;

        readonly EcsFilterInject<Inc<Health>, Exc<Dead>> _healthFilter = default;
        readonly EcsFilterInject<Inc<InterfaceComponent>> _interfaceFilter = default;

        readonly EcsPoolInject<InterfaceComponent> _interfacePool = default;
        readonly EcsPoolInject<Health> _healthPool = default;

        public void Run (IEcsSystems systems)
        {
            foreach (var entity in _healthFilter.Value)
            {
                ref var health = ref _healthPool.Value.Get(entity);

                if (health.HealthbarMB == null)
                {
                    CreateHealthbar(ref health);
                }

                health.HealthbarMB.transform.position = Camera.main.WorldToScreenPoint(health.PointForHealthbar.position);
            }
        }

        private void CreateHealthbar(ref Health health)
        {
            foreach (var intefaceEntity in _interfaceFilter.Value)
            {
                ref var interfaceComponent = ref _interfacePool.Value.Get(intefaceEntity);

                health.HealthbarMB = GameObject.Instantiate(_gameState.Value.UiConfig.HealthbarMBPrefab, interfaceComponent.MainUiPrefabMB.GameplayCanvas.transform);
            }
        }
    }
}