using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class DyingPlayerSystem : IEcsRunSystem
    {
        readonly EcsCustomInject<GameState> _gameState;

        readonly EcsFilterInject<Inc<DyingEvent>> _eventFilter = default;

        readonly EcsPoolInject<DyingEvent> _eventPool = default;

        readonly EcsPoolInject<Player> _playerPool = default;
        readonly EcsPoolInject<Movable> _movablePool = default;

        public void Run (IEcsSystems systems)
        {
            foreach (var eventEntity in _eventFilter.Value)
            {
                ref var eventComponent = ref _eventPool.Value.Get(eventEntity);

                if (!_playerPool.Value.Has(eventComponent.Entity))
                {
                    continue;
                }

                if (_movablePool.Value.Has(eventEntity))
                {
                    _movablePool.Value.Del(eventEntity);
                }

                _gameState.Value.KillPlayer();
            }
        }
    }
}