using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsedCardAction : CardInterfaceAction
{
    public static UsedCardAction GetInstance() { return new UsedCardAction(); }

    float _elapsedTime;
    float _startFloat;

    public override void Start(CardInterface owner)
    {
        base.Start(owner);
        owner.Anim.enabled = false;
        _startFloat = Owner.DissolveValue;
        UseCardLogData.GetInstance().Init(Owner);
    }

    public override void Update()
    {
        base.Update();
        _elapsedTime += Time.unscaledDeltaTime;
        Owner.DissolveValue = _startFloat - _elapsedTime * 2f;
        if (_startFloat - _elapsedTime * 2f <= 0f)
        {
            GameObject.Destroy(Owner.gameObject);
        }
    }
}
