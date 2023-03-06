using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class CreateUIDamageNumberEventSystem : IEcsRunSystem
    {
        readonly EcsCustomInject<GameState> _gameState;

        readonly EcsWorldInject _world;

        readonly EcsFilterInject<Inc<CreateUIDamageNumberEvent>> _eventFilter = default;
        readonly EcsFilterInject<Inc<InterfaceComponent>> _interfaceFilter = default;

        readonly EcsPoolInject<CreateUIDamageNumberEvent> _eventPool = default;

        readonly EcsPoolInject<DamageNumber> _damageNumberPool = default;
        readonly EcsPoolInject<InterfaceComponent> _interfacePool = default;

        private int _eventEntity = GameState.NULL_ENTITY;

        public void Run (IEcsSystems systems)
        {
            foreach (var eventEntity in _eventFilter.Value)
            {
                _eventEntity = eventEntity;

                ref var eventComponent = ref _eventPool.Value.Get(_eventEntity);

                foreach (var interfaceEntity in _interfaceFilter.Value)
                {
                    ref var interfaceComponent = ref _interfacePool.Value.Get(interfaceEntity);

                    var damageNumberEntity = _world.Value.NewEntity();
                    ref var damageNumber = ref _damageNumberPool.Value.Add(damageNumberEntity);
                    damageNumber.UIDamageNumberMB = GameObject.Instantiate(_gameState.Value.UiConfig.UIDamageNumberMB, interfaceComponent.MainUiPrefabMB.GameplayCanvas.transform);
                    damageNumber.UIDamageNumberMB.transform.position = Camera.main.WorldToScreenPoint(eventComponent.StartPoint);
                    damageNumber.UIDamageNumberMB.Text.text = eventComponent.DamageValue.ToString();
                    damageNumber.LifeTime = _gameState.Value.UiConfig.UIDamageNumberLifeTime;
                }

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