using UnityEngine;
using System;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "AudioStorage", menuName = "Storages/Audio")]
public class AudioStorage : ScriptableObject
{
    public Effects Effects;
    [Space]
    public Ambients Ambients;
    [Space]
    public LevelEndAudio LevelEnd;
    [Space]
    public UiAudio UIAudio;
}

#region Effects

[Serializable]
public class Effects
{
    public Zombies Zombies;
    public Shoots Shoots;
    public Explosion Explosion;
}

[Serializable]
public class Zombies
{
    public AudioClip[] BodyHit;
    public AudioClip[] Screams;
}

[Serializable]
public class Shoots
{
    public AudioClip BlankShot;
}

[Serializable]
public class Explosion
{
    public AudioClip ExplosionNapalm;
    public AudioClip ExplosionGranade;
    public AudioClip FlyNapalm;
}

#endregion

#region Ambients

[Serializable]
public class Ambients
{
    public AudioClip ForsakenTrailerMusic;
    public AudioClip EpicEyesOfGlory;
    public AudioClip AnsiaOrchestra;
}

#endregion

#region LevelEnd

[Serializable]
public class LevelEndAudio
{
    public AudioClip Win;
    public AudioClip Lose;
}

#endregion

#region UiAudio

[Serializable]
public class UiAudio
{
    public AudioClip ButtonTap;
    public AudioClip AddExp;
}

#endregion