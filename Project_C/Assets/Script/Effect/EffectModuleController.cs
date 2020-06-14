using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EffectModuleController : MonoBehaviour
{
    public List<EffectModule> ManagedEffects { get; protected set; }

    private void Awake()
    {
        ManagedEffects = new List<EffectModule>();
    }
}
