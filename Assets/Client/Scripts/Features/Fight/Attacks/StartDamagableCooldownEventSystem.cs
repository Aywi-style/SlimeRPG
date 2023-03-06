using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class StartDamagableCooldownEventSystem : IEcsRunSystem
    {
        readonly EcsCustomInject<GameState> _gameState;

        readonly EcsWorldInject _world;

        readonly EcsFilterInject<Inc<StartDamagableCooldownEvent>> _eventFilter = default;

        readonly EcsPoolInject<StartDamagableCooldownEvent> _eventPool = default;

        readonly EcsPoolInject<DamagableCooldown> _damagableCooldownPool = default;
        readonly EcsPoolInject<Damagable> _damagablePool = default;

        private readonly int _oneHit = 1;

        private int _eventEntity = GameState.NULL_ENTITY;

        public void Run (IEcsSystems systems)
        {
            if (_eventFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }

            foreach (var eventEntity in _eventFilter.Value)
            {
                _eventEntity = eventEntity;

                ref var eventComponent = ref _eventPool.Value.Get(_eventEntity);

                if (!_damagablePool.Value.Has(eventComponent.Entity))
                {
                    DeleteEvent();
                    continue;
                }

                if (!_damagableCooldownPool.Value.Has(eventComponent.Entity))
                {
                    _damagableCooldownPool.Value.Add(eventComponent.Entity);
                }

                ref var damagable = ref _damagablePool.Value.Get(eventComponent.Entity);

                ref var damagableCooldown = ref _damagableCooldownPool.Value.Get(eventComponent.Entity);
                damagableCooldown.MaxValue = _oneHit / damagable.AttackSpeed;
                damagableCooldown.CurrentValue = damagableCooldown.MaxValue;

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