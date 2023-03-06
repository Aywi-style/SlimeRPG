using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class DyingEnemySystem : IEcsRunSystem
    {
        readonly EcsCustomInject<GameState> _gameState;

        readonly EcsFilterInject<Inc<DyingEvent>> _eventFilter = default;

        readonly EcsPoolInject<DyingEvent> _eventPool = default;

        readonly EcsPoolInject<Enemy> _enemyPool = default;
        readonly EcsPoolInject<Health> _healthPool = default;
        readonly EcsPoolInject<View> _viewPool = default;
        readonly EcsPoolInject<RewardGiver> _rewardGiverPool = default;

        public void Run (IEcsSystems systems)
        {
            foreach (var eventEntity in _eventFilter.Value)
            {
                ref var eventComponent = ref _eventPool.Value.Get(eventEntity);

                if (!_enemyPool.Value.Has(eventComponent.Entity))
                {
                    continue;
                }

                _gameState.Value.IncreaseKillCount();

                if (_rewardGiverPool.Value.Has(eventComponent.Entity))
                {
                    ref var rewardGiver = ref _rewardGiverPool.Value.Get(eventComponent.Entity);

                    _gameState.Value.MoneyValueChange(rewardGiver.Value);
                }

                if (_healthPool.Value.Has(eventComponent.Entity))
                {
                    ref var health = ref _healthPool.Value.Get(eventComponent.Entity);

                    if (health.HealthbarMB != null)
                    {
                        GameObject.Destroy(health.HealthbarMB.gameObject);
                    }
                }

                if (_viewPool.Value.Has(eventComponent.Entity))
                {
                    ref var view = ref _viewPool.Value.Get(eventComponent.Entity);
                    view.GameObject.SetActive(false);
                }
            }
        }
    }
}