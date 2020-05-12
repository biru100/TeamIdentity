using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BurnCardAction : CardInterfaceAction
{
    public static BurnCardAction GetInstance() { return new BurnCardAction(); }

    float _elapsedTime;
    float _startFloat;


    public Action OnFinish { get; set; }

    public override void Start(CardInterface owner)
    {
        base.Start(owner);
        owner.Anim.enabled = true;
        owner.Anim.Play("BurnCard");
    }

    public override void Update()
    {
        base.Update();

        Owner.DissolveValue = Owner.DissolveValue;

        if (Owner.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f)
        {
            OnFinish?.Invoke();
            GameObject.Destroy(Owner);
        }
    }
}
