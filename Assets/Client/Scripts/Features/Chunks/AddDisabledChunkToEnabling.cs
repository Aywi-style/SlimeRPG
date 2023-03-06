using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client
{
    sealed class AddDisabledChunkToEnabling : IEcsRunSystem
    {
        readonly EcsCustomInject<GameState> _gameState;

        readonly EcsWorldInject _world;

        readonly EcsFilterInject<Inc<Player, View>> _playerFilter = default;

        readonly EcsPoolInject<ChunkEandomEnablingEvent> _chunkRandomEnablingEventPool = default;

        readonly EcsPoolInject<View> _viewPool = default;
        readonly EcsPoolInject<ChunkComponent> _chunkPool = default;

        public void Run (IEcsSystems systems)
        {
            if (_playerFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }

            ref var chunk = ref _chunkPool.Value.Get(_gameState.Value.LastEnabledChunkEntity);
            ref var playerView = ref _viewPool.Value.Get(_playerFilter.Value.GetRawEntities()[0]);

            if (chunk.ChunkMB.ChunkEnd.position.z <= playerView.Transform.position.z + _gameState.Value.ChunksConfig.EnablingChunkPositionOffset)
            {
                _chunkRandomEnablingEventPool.Value.Add(_world.Value.NewEntity());
            }
        }
    }
}