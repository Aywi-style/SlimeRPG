using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class ChunkDisablingEventSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<ChunkDisablingEvent>> _eventFilter = default;

        readonly EcsPoolInject<ChunkDisablingEvent> _eventPool = default;

        readonly EcsPoolInject<DisabledChunk> _disabledChunkPool = default;
        readonly EcsPoolInject<View> _viewPool = default;

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

                ref var chunkDisablingEvent = ref _eventPool.Value.Get(_eventEntity);

                if (_disabledChunkPool.Value.Has(chunkDisablingEvent.DisablingEntity))
                {
                    DeleteEvent();
                    continue;
                }

                ref var view = ref _viewPool.Value.Get(chunkDisablingEvent.DisablingEntity);
                view.GameObject.SetActive(false);

                _disabledChunkPool.Value.Add(chunkDisablingEvent.DisablingEntity);

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