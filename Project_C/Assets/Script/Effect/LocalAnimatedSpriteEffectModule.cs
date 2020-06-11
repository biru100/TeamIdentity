using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LocalAnimatedSpriteEffectModule : EffectModule
{
    public static LocalAnimatedSpriteEffectModule CreateEffect(Character owner, Transform parent, string name)
    {
        LocalAnimatedSpriteEffectModule effect = 
            Instantiate(ResourceManager.GetResource<GameObject>("Effects/" + name), parent)
            .GetComponent<LocalAnimatedSpriteEffectModule>();
        effect.Owner = owner;
        return effect;
    }

    public Character Owner { get; protected set; }
    public Animator Anim { get; protected set; }

    private void Awake()
    {
        Anim = GetComponent<Animator>();
    }

    public override void Play(string name)
    {
        gameObject.SetActive(true);
        Anim.Play(name + "_" + AnimUtil.GetRenderAngle(Owner.transform.rotation));
    }

    public override void Stop()
    {
        gameObject.SetActive(false);
    }

}
