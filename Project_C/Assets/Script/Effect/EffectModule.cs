using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectModule : MonoBehaviour
{
    public enum ModuleType
    {
        E_AnimatedSprite,
        E_ParticleSystem
    }

    public ModuleType EffectType { get; set; }

    public virtual void Play() { }
    public virtual void Play(string name) { }

    public virtual void Stop() { }
}
