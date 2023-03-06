using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class InitInterface : IEcsInitSystem
    {
        readonly EcsCustomInject<GameState> _state = default;

        readonly EcsWorldInject _world = default;

        readonly EcsPoolInject<InterfaceComponent> _interfacePool = default;

        public void Init (IEcsSystems systems)
        {
            int interfaceEntity = _world.Value.NewEntity();

            ref var interfaceComponent = ref _interfacePool.Value.Add(interfaceEntity);

            interfaceComponent.MainUiPrefabMB = GameObject.Instantiate(_state.Value.UiConfig.MainUiPrefabMB);
        }
    }
}