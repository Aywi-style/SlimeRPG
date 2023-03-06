using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class DamageNumberHandler : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<DamageNumber>> _damageNumberFilter = default;

        readonly EcsPoolInject<DamageNumber> _damageNumberPool = default;

        private float _offset = 20f;

        private int _currentEntity;

        public void Run (IEcsSystems systems)
        {
            foreach (var entity in _damageNumberFilter.Value)
            {
                _currentEntity = entity;

                ref var damageNumber = ref _damageNumberPool.Value.Get(entity);

                if (damageNumber.LifeTime <= 0)
                {
                    Delete(ref damageNumber);

                    continue;
                }

                damageNumber.LifeTime -= Time.deltaTime;
                damageNumber.UIDamageNumberMB.transform.position += Vector3.up * _offset * Time.deltaTime;
            }
        }

        private void Delete(ref DamageNumber damageNumber)
        {
            GameObject.Destroy(damageNumber.UIDamageNumberMB.gameObject);
            _damageNumberPool.Value.Del(_currentEntity);
        }
    }
}