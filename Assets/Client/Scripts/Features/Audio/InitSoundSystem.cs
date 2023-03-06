using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class InitSoundSystem : IEcsInitSystem
    {
        readonly EcsWorldInject _world;

        readonly EcsSharedInject<GameState> _gameState;

        readonly EcsPoolInject<ChangeAmbientEvent> _changeAmbientEventPool = default;

        readonly EcsPoolInject<SoundSystemComponent> _soundSystemPool = default;

        private AudioClip _actualAudioClip;

        private int _soundSystemEntity;

        public void Init (IEcsSystems systems)
        {
            _gameState.Value.AudioPack.AudioSnapshots.Normal.TransitionTo(0.5f);

            var soundSystems = GameObject.FindObjectsOfType<SoundSystemMB>();

            foreach (var soundSystem in soundSystems)
            {
                _soundSystemEntity = _world.Value.NewEntity();

                ref var soundSystemComponent = ref _soundSystemPool.Value.Add(_soundSystemEntity);

                if (soundSystem.GetComponentInChildren<AmbientMB>().TryGetComponent<AudioSource>(out var ambientSource))
                {
                    soundSystemComponent.AmbientSource = ambientSource;
                }

                if (soundSystem.GetComponentInChildren<LevelEndMB>().TryGetComponent<AudioSource>(out var levelEndSource))
                {
                    soundSystemComponent.LevelEndSource = levelEndSource;
                }

                if (soundSystem.GetComponentInChildren<EffectsAudioMB>().TryGetComponent<AudioSource>(out var effectsSource))
                {
                    soundSystemComponent.EffectsSource = effectsSource;
                }

                CheckActualAudioClip();

                CheckNeedsChangeAmbient();

                if (_gameState.Value.IsNeedChangeEmbient)
                {
                    InvokeChangeAmbientEvent();
                }
            }
        }

        private void CheckActualAudioClip()
        {
            switch (_gameState.Value.AmbientType)
            {
                case AmbientType.AnsiaOrchestra:
                    _actualAudioClip = _gameState.Value.AudioPack.AudioStorage.Ambients.AnsiaOrchestra;
                    break;
                case AmbientType.EpicEyesOfGlory:
                    _actualAudioClip = _gameState.Value.AudioPack.AudioStorage.Ambients.EpicEyesOfGlory;
                    break;
                case AmbientType.ForsakenTrailerMusic:
                    _actualAudioClip = _gameState.Value.AudioPack.AudioStorage.Ambients.ForsakenTrailerMusic;
                    break;
                default:
                    _actualAudioClip = null;
                    Debug.LogWarning("This scene haven't ambient!");
                    break;
            }
        }

        private void CheckNeedsChangeAmbient()
        {
            ref var soundSystemComponent = ref _soundSystemPool.Value.Get(_soundSystemEntity);

            _gameState.Value.IsNeedChangeEmbient = soundSystemComponent.AmbientSource.clip != _actualAudioClip;
        }

        private void InvokeChangeAmbientEvent()
        {
            _changeAmbientEventPool.Value.Add(_world.Value.NewEntity()).Invoke(_actualAudioClip);
        }
    }
}