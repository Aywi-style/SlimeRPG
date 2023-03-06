using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using System.Collections.Generic;

namespace Client
{
    sealed class SpawnAllChunks : IEcsInitSystem
    {
        readonly EcsCustomInject<GameState> _gameState;

        readonly EcsWorldInject _world;

        readonly EcsPoolInject<ChunkComponent> _chunkPool = default;
        readonly EcsPoolInject<View> _viewPool = default;

        private Vector3 _lastChunkEndPosition = Vector3.zero;
        private Quaternion _lastChunkEndQuaternion = Quaternion.identity;

        public void Init (IEcsSystems systems)
        {
            _lastChunkEndPosition.z += _gameState.Value.ChunksConfig.StartChunkPositionOffset;

            _gameState.Value.AllEnabledChunks = new Queue<int>(_gameState.Value.ChunksConfig.MaxChunkCount);

            for (int i = 0; i < _gameState.Value.ChunksConfig.MaxChunkCount; i++)
            {
                var currentSpawningChunk = i % _gameState.Value.ChunksConfig.ChunksMBs.Length;

                var chunkEntity = _world.Value.NewEntity();

                _gameState.Value.AllEnabledChunks.Enqueue(chunkEntity);

                ref var chunk = ref _chunkPool.Value.Add(chunkEntity);
                chunk.ChunkMB = GameObject.Instantiate(_gameState.Value.ChunksConfig.ChunksMBs[currentSpawningChunk], _lastChunkEndPosition, _lastChunkEndQuaternion);

                ref var view = ref _viewPool.Value.Add(chunkEntity);
                view.GameObject = chunk.ChunkMB.gameObject;
                view.Transform = view.GameObject.transform;

                _lastChunkEndPosition = chunk.ChunkMB.ChunkEnd.position;
                _lastChunkEndQuaternion = chunk.ChunkMB.ChunkEnd.rotation;

                if (i == _gameState.Value.ChunksConfig.MaxChunkCount - 1)
                {
                    _gameState.Value.LastEnabledChunkEntity = chunkEntity;
                }
            }
        }
    }
}