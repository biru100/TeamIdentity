using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : BehaviorSingleton<CameraManager>
{
    public Animator Anim { get; protected set; }

    protected override void Awake()
    {
        base.Awake();

        Anim = GetComponent<Animator>();
    }

    public static void PlayAnim(string name, int angle)
    {
        Instance.Anim.Play(name + "_" + angle, 0, 0f);
    }
}
