using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class InitPools : IEcsInitSystem
    {
        readonly EcsSharedInject<GameState> _gameState;

        private int _basedZombiesCount = 1000;
        private int _basedBulletCount = 100;
        Vector3 _spawnPoint = new Vector3(0, 0, 100f);

        public void Init (IEcsSystems systems)
        {
            _gameState.Value.ActivePools = new AllPools();
            _gameState.Value.ActivePools.ZombiesPool = new RandomPool(_gameState.Value.AllPools.ZombiesPool.Prefabs, _spawnPoint, _basedZombiesCount, parentName: "BasedZombies");

            _gameState.Value.ActivePools.BulletMissilesPool = new Pool(_gameState.Value.AllPools.BulletMissilesPool.Prefab, _spawnPoint, _basedBulletCount, parentName: "BulletMissiles");
        }
    }
}