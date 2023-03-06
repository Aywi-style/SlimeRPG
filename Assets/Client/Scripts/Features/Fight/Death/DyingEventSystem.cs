using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class DyingEventSystem : IEcsRunSystem
    {
        readonly EcsCustomInject<GameState> _gameState;

        readonly EcsFilterInject<Inc<DyingEvent>> _eventFilter = default;

        readonly EcsPoolInject<DyingEvent> _eventPool = default;

        readonly EcsPoolInject<Dead> _deadPool = default;

        private int _eventEntity = GameState.NULL_ENTITY;

        public void Run (IEcsSystems systems)
        {
            foreach (var eventEntity in _eventFilter.Value)
            {
                _eventEntity = eventEntity;

                ref var eventComponent = ref _eventPool.Value.Get(_eventEntity);

                if (!_deadPool.Value.Has(eventComponent.Entity))
                {
                    _deadPool.Value.Add(eventComponent.Entity);
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