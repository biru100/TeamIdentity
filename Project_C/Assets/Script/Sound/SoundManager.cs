using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : BehaviorSingleton<SoundManager>
{
    public static readonly int BGMAudioSourceCount = 3;
    public static readonly int EffectAudioSourceCount = 15;

    protected int CurrentBgmAudioSourceIndex { get; set; }
    protected int CurrentEffectAudioSourceIndex { get; set; }

    protected List<AudioSource> BGMAudioSource;
    protected List<AudioSource> EffectAudioSource;

    protected override void Init()
    {
        base.Init();

        BGMAudioSource = new List<AudioSource>();

        for (int i = 0; i < CurrentBgmAudioSourceIndex; ++i)
        {
            BGMAudioSource.Add(gameObject.AddComponent<AudioSource>());
        }

        EffectAudioSource = new List<AudioSource>();

        for (int i = 0; i < EffectAudioSourceCount; ++i)
        {
            EffectAudioSource.Add(gameObject.AddComponent<AudioSource>());
        }
    }


    public static void PlayEffectSound(string path)
    {
        Instance.EffectAudioSource[Instance.CurrentEffectAudioSourceIndex].clip = ResourceManager.GetResource<AudioClip>(path);
        Instance.EffectAudioSource[Instance.CurrentEffectAudioSourceIndex].Play();
        Instance.CurrentEffectAudioSourceIndex = (Instance.CurrentEffectAudioSourceIndex + 1 % EffectAudioSourceCount);
    }
}
