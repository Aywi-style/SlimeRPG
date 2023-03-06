using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class PlayerAttackSystem : IEcsRunSystem
    {
        readonly EcsCustomInject<GameState> _gameState;

        readonly EcsWorldInject _world = default;

        readonly EcsFilterInject<Inc<Player, Targetable, Damagable>, Exc<DamagableCooldown>> _playerFilter = default;

        readonly EcsPoolInject<LaunchProjectileEvent> _launchProjectileEventPool = default;
        readonly EcsPoolInject<StartDamagableCooldownEvent> _startDamagableCooldownEventPool = default;

        readonly EcsPoolInject<Player> _playerPool = default;
        readonly EcsPoolInject<Targetable> _targetablePool = default;
        readonly EcsPoolInject<Damagable> _damagablePool = default;
        readonly EcsPoolInject<Enemy> _enemyPool = default;
        readonly EcsPoolInject<Dead> _deadPool = default;

        public void Run (IEcsSystems systems)
        {
            foreach (var playerEntity in _playerFilter.Value)
            {
                if (_deadPool.Value.Has(playerEntity))
                {
                    continue;
                }

                ref var targetable = ref _targetablePool.Value.Get(playerEntity);

                if (targetable.TargetedEnemy == GameState.NULL_ENTITY)
                {
                    continue;
                }

                ref var player = ref _playerPool.Value.Get(playerEntity);
                ref var damagable = ref _damagablePool.Value.Get(playerEntity);

                ref var enemy = ref _enemyPool.Value.Get(targetable.TargetedEnemy);

                ref var launchProjectileEvent = ref _launchProjectileEventPool.Value.Add(_world.Value.NewEntity());
                launchProjectileEvent.FirePoint = player.PlayerMB.FirePoint;
                launchProjectileEvent.PointForFiring = enemy.EnemyMB.PointForFiring;
                launchProjectileEvent.Speed = _gameState.Value.PlayerConfig.ProjectileSpeed;
                launchProjectileEvent.Damage = damagable.Value;
                launchProjectileEvent.TargetEntity = targetable.TargetedEnemy;

                ref var startDamagableCooldownEvent = ref _startDamagableCooldownEventPool.Value.Add(_world.Value.NewEntity());
                startDamagableCooldownEvent.Entity = playerEntity;
            }
        }
    }
}