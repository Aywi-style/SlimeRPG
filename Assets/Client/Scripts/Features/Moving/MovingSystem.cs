using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class MovingSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<Movable, View>, Exc<Standing>> _movableFilter = default;

        readonly EcsPoolInject<Movable> _movablePool = default;
        readonly EcsPoolInject<View> _viewPool = default;

        public void Run (IEcsSystems systems)
        {
            foreach (var entity in _movableFilter.Value)
            {
                ref var view = ref _viewPool.Value.Get(entity);
                ref var movable = ref _movablePool.Value.Get(entity);

                view.Transform.Translate(movable.Direction.normalized * movable.Speed * Time.deltaTime);
            }
        }
    }
}