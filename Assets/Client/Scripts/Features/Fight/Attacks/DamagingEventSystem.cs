using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class DamagingEventSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;

        readonly EcsSharedInject<GameState> _state;

        readonly EcsFilterInject<Inc<DamagingEvent>> _eventFilter = default;

        readonly EcsPoolInject<DamagingEvent> _eventPool = default;
        readonly EcsPoolInject<DyingEvent> _dyingPool = default;
        readonly EcsPoolInject<CreateUIDamageNumberEvent> _createUIDamageNumberEventPool = default;

        readonly EcsPoolInject<Health> _healthPool = default;
        readonly EcsPoolInject<View> _viewPool = default;
        readonly EcsPoolInject<Dead> _deadPool = default;

        private int _eventEntity = GameState.NULL_ENTITY;

        public void Run (IEcsSystems systems)
        {
            foreach (var eventEntity in _eventFilter.Value)
            {
                _eventEntity = eventEntity;

                ref var eventComponent = ref _eventPool.Value.Get(eventEntity);

                if (eventComponent.TargetEntity == -1)
                {
                    Debug.Log("При проведении DamagingEvent пришла -1 энтити");
                    DeleteEvent();
                    continue;
                }

                if (_deadPool.Value.Has(eventComponent.TargetEntity))
                {
                    DeleteEvent();
                    continue;
                }

                if (!_healthPool.Value.Has(eventComponent.TargetEntity))
                {
                    DeleteEvent();
                    continue;
                }

                ref var health = ref _healthPool.Value.Get(eventComponent.TargetEntity);
                ref var view = ref _viewPool.Value.Get(eventComponent.TargetEntity);

                if (eventComponent.DamageValue > health.CurrentValue)
                {
                    eventComponent.DamageValue = health.CurrentValue;
                }

                health.CurrentValue -= eventComponent.DamageValue;
                health.HealthbarMB?.UpdateBar(health.MaxValue, health.CurrentValue);

                ref var createUIDamageNumberEvent = ref _createUIDamageNumberEventPool.Value.Add(_world.Value.NewEntity());
                createUIDamageNumberEvent.DamageValue = eventComponent.DamageValue;
                createUIDamageNumberEvent.StartPoint = health.PointForHealthbar.position;

                if (health.CurrentValue <= 0)
                {
                    DieEvent(eventComponent.TargetEntity);
                }

                DeleteEvent();
            }
        }

        private void DeleteEvent()
        {
            _eventPool.Value.Del(_eventEntity);

            _eventEntity = GameState.NULL_ENTITY;
        }

        private void DieEvent(int entity)
        {
            ref var dyingEvent = ref _dyingPool.Value.Add(_world.Value.NewEntity());
            dyingEvent.Entity = entity;
        }
    }
}