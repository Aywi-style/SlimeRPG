using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEditor;

namespace Client
{
    sealed class ChunkRandomEnablingEventSystem : IEcsRunSystem
    {
        readonly EcsCustomInject<GameState> _gameState;

        readonly EcsFilterInject<Inc<ChunkEandomEnablingEvent>> _eventFilter = default;
        readonly EcsFilterInject<Inc<ChunkComponent, DisabledChunk, View>> _disabledChunkFilter = default;

        readonly EcsPoolInject<ChunkEandomEnablingEvent> _eventPool = default;

        readonly EcsPoolInject<View> _viewPool = default;
        readonly EcsPoolInject<ChunkComponent> _chunkPool = default;
        readonly EcsPoolInject<DisabledChunk> _disabledChunkPool = default;

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

                var disabledEntitiesCount = _disabledChunkFilter.Value.GetEntitiesCount();

                if (disabledEntitiesCount <= 0)
                {
                    DeleteEvent();
                    continue;
                }

                var disabledRawEntities = _disabledChunkFilter.Value.GetRawEntities();

                var disabledEntity = disabledRawEntities[Random.Range(0, disabledEntitiesCount)];

                _disabledChunkPool.Value.Del(disabledEntity);

                ref var disabledChunkView = ref _viewPool.Value.Get(disabledEntity);
                disabledChunkView.GameObject.SetActive(true);

                ref var disabledChunk = ref _chunkPool.Value.Get(disabledEntity);

                ref var lastChunk = ref _chunkPool.Value.Get(_gameState.Value.LastEnabledChunkEntity);
                disabledChunkView.Transform.position = lastChunk.ChunkMB.ChunkEnd.position;

                _gameState.Value.LastEnabledChunkEntity = disabledEntity;
                _gameState.Value.AllEnabledChunks.Enqueue(disabledEntity);

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