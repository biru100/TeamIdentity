using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DrawCardAction : CardInterfaceAction
{
    public static DrawCardAction GetInstance() { return new DrawCardAction(); }

    float _startFloat;

    public Action OnFinish { get; set; }

    public override void Start(CardInterface owner)
    {
        base.Start(owner);
        owner.Anim.enabled = true;
        owner.Anim.Play("DrawCard");
    }

    public override void Update()
    {
        base.Update();

        if (Owner.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f)
            Owner.CurrentAction = HandCardAction.GetInstance();
    }

    public override void Finish()
    {
        base.Finish();
        OnFinish?.Invoke();
    }
}
