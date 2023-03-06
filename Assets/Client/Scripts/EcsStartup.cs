using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public sealed class EcsStartup : MonoBehaviour
    {
        public EcsWorld World { private set; get; }

        private GameState _gameState;
        private EcsSystems _playableSystems, _chunksSystems;

        [field: Space]
        [field: SerializeField] public AllPools AllPools { private set; get; }
        [field: SerializeField] public ChunksConfig ChunksConfig { private set; get; }
        [field: SerializeField] public PlayerConfig PlayerConfig { private set; get; }
        [field: SerializeField] public EnemyConfig EnemyConfig { private set; get; }
        [field: SerializeField] public UiConfig UiConfig { private set; get; }
        [field: SerializeField] public UpgradesConfig UpgradesConfig { private set; get; }

        [field: Space]
        [field: SerializeField] public AmbientType AmbientType { private set; get; }
        [field: SerializeField] public AudioMixer AudioPack { private set; get; }

        private void Start()
        {
            World = new EcsWorld();

            _gameState = GameState.InitializeNew(this);

            _playableSystems = new EcsSystems(World);
            _chunksSystems = new EcsSystems(World);

            _playableSystems
                .Add(new StartScene())

                .Add(new InitInterface())

                .Add(new SpawnAllChunks())
                .Add(new PlayerSpawnEventSystem())
                .Add(new EnemySpawnSystem())
                .Add(new EnemySpawnEventSystem())

                .Add(new HealthbarSystem())

                .Add(new StandingSystem())

                .Add(new MovingSystem())

                .Add(new PlayerDetectingSystem())
                .Add(new TargetingOnDetectedEnemy())
                .Add(new ClearPlayerTargetedEnemy())

                .Add(new PlayerAttackSystem())
                .Add(new EnemyChooseTargetSystem())
                .Add(new EnemyAttackSystem())

                .Add(new LaunchProjectileEventSystem())
                .Add(new ProjectileFlyingSystem())

                .Add(new HealthRecoverySystem())
                .Add(new DamagingEventSystem())
                .Add(new CreateUIDamageNumberEventSystem())
                .Add(new DamageNumberHandler())

                .Add(new UpgradeEventSystem())

                .Add(new StartDamagableCooldownEventSystem())
                .Add(new DamagableCooldownSystem())

                .Add(new DyingEnemySystem())
                .Add(new DyingPlayerSystem())
                .Add(new DyingEventSystem())
                ;

            _chunksSystems
                .Add(new AddEnabledChunkToDisabling())
                .Add(new ChunkDisablingEventSystem())
                .Add(new AddDisabledChunkToEnabling())
                .Add(new ChunkRandomEnablingEventSystem())
                ;
#if UNITY_EDITOR
            _playableSystems.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
                ;
#endif

            InjectAllSystems(_playableSystems, _chunksSystems);
            InitAllSystems(_playableSystems, _chunksSystems);
        }

        private void Update()
        {
            _playableSystems?.Run();
            _chunksSystems?.Run();
        }

        private void OnDestroy()
        {
            OnDestroyAllSystems(_playableSystems, _chunksSystems);

            if (World != null)
            {
                World.Destroy();
                World = null;
            }
        }

        private void InjectAllSystems(params EcsSystems[] systems)
        {
            foreach (var system in systems)
            {
                system.Inject(_gameState);
            }
        }

        private void InitAllSystems(params EcsSystems[] systems)
        {
            foreach (var system in systems)
            {
                system.Init();
            }
        }

        private void OnDestroyAllSystems(params EcsSystems[] systems)
        {
            for (int i = 0; i < systems.Length; i++)
            {
                if (systems[i] != null)
                {
                    systems[i].Destroy();
                    systems[i] = null;
                }
            }
        }
    }
}
