using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class StartScene : IEcsInitSystem
    {
        readonly EcsCustomInject<GameState> _gameState;

        readonly EcsWorldInject _world;

        readonly EcsPoolInject<PlayerSpawnEvent> _playerSpawnEventPool = default;

        public void Init(IEcsSystems systems)
        {
            _playerSpawnEventPool.Value.Add(_world.Value.NewEntity());
        }
    }
}