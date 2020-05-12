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
        _startFloat = Owner.FrontSide.material.GetFloat("_DissolveValue");
    }

    public override void Update()
    {
        base.Update();
        _elapsedTime += Time.unscaledDeltaTime;
        Owner.FrontSide.material.SetFloat("_DissolveValue", Mathf.Max(_startFloat - _elapsedTime * 2f, 0f));
        if(_startFloat - _elapsedTime * 2f <= 0f)
            GameObject.Destroy(Owner.gameObject);
    }
}
