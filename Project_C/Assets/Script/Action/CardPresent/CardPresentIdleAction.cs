using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CardPresentIdleAction : CharacterAction
{

public static CardPresentIdleAction GetInstance() { return ObjectPooling.PopObject<CardPresentIdleAction>(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
}

public override void UpdateAction()
{
base.UpdateAction();
}

public override void FinishAction()
{
base.FinishAction();
}
}
