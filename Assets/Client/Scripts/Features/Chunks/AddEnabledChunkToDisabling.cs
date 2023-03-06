using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class AddEnabledChunkToDisabling : IEcsRunSystem
    {
        readonly EcsCustomInject<GameState> _gameState;

        readonly EcsWorldInject _world;

        readonly EcsFilterInject<Inc<Player, View>> _playerFilter = default;

        readonly EcsPoolInject<View> _viewPool = default;
        readonly EcsPoolInject<ChunkComponent> _chunkPool = default;
        readonly EcsPoolInject<ChunkDisablingEvent> _disablingChunkPool = default;

        public void Run (IEcsSystems systems)
        {
            if (_playerFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }

            if (_gameState.Value.AllEnabledChunks.TryPeek(out int chunkEntity))
            {
                ref var playerView = ref _viewPool.Value.Get(_playerFilter.Value.GetRawEntities()[0]);

                ref var chunk = ref _chunkPool.Value.Get(chunkEntity);

                if (playerView.Transform.position.z > chunk.ChunkMB.ChunkEnd.position.z + _gameState.Value.ChunksConfig.DisablingChunkPositionOffset)
                {
                    ref var chunkDisablingEvent = ref _disablingChunkPool.Value.Add(_world.Value.NewEntity());
                    chunkDisablingEvent.DisablingEntity = chunkEntity;

                    _gameState.Value.AllEnabledChunks.Dequeue();
                }
            }
            else
            {
                Debug.LogError("AllActiveChunks is empty!");
            }
            
        }
    }
}