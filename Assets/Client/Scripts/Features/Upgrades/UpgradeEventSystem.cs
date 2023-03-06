using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class UpgradeEventSystem : IEcsRunSystem
    {
        readonly EcsCustomInject<GameState> _gameState;

        readonly EcsFilterInject<Inc<UpgradeEvent>> _eventFilter = default;

        readonly EcsPoolInject<UpgradeEvent> _eventPool = default;

        readonly EcsPoolInject<Health> _healthPool = default;
        readonly EcsPoolInject<HealthRecovery> _healthRecoveryPool = default;
        readonly EcsPoolInject<Damagable> _damagablePool = default;

        private int _eventEntity = GameState.NULL_ENTITY;

        public void Run (IEcsSystems systems)
        {
            foreach (var eventEntity in _eventFilter.Value)
            {
                _eventEntity = eventEntity;

                ref var eventComponent = ref _eventPool.Value.Get(_eventEntity);

                var playerEntity = _gameState.Value.PlayerEntity;

                ref var damagable = ref _damagablePool.Value.Get(playerEntity);
                ref var health = ref _healthPool.Value.Get(playerEntity);
                ref var healthRecovery = ref _healthRecoveryPool.Value.Get(playerEntity);

                switch (eventComponent.UpgradeType)
                {
                    case UpgradeType.Attack:
                        damagable.Value += Mathf.RoundToInt(eventComponent.Value);
                        break;

                    case UpgradeType.Health:
                        health.MaxValue += Mathf.RoundToInt(eventComponent.Value);
                        health.CurrentValue += Mathf.RoundToInt(eventComponent.Value);
                        break;

                    case UpgradeType.HealthRecovery:
                        healthRecovery.Value += eventComponent.Value;
                        break;

                    case UpgradeType.AttackSpeed:
                        damagable.AttackSpeed += eventComponent.Value;
                        break;

                    default:
                        break;
                }

                DeleteEvent();
            }
        }

        private void DeleteEvent()
        {
            _eventPool.Value.Del(_eventEntity);

            _eventEntity = GameState.NULL_ENTITY;
        }
    }
}