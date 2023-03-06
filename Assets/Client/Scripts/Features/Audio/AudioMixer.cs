using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Client
{
    [Serializable]
    public class AudioMixer
    {
        public AudioMixerGroup MasterGroup;
        [Space]
        public AudioStorage AudioStorage;
        [Space]
        public Snapshots AudioSnapshots;

        [Serializable]
        public class Snapshots
        {
            public AudioMixerSnapshot LevelEnd;
            public AudioMixerSnapshot Normal;
            public AudioMixerSnapshot TutorialPause;
        }
    }
}
