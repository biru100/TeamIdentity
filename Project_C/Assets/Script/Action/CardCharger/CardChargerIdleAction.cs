using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CardChargerIdleAction : CharacterAction
{

public static CardChargerIdleAction GetInstance() { return ObjectPooling.PopObject<CardChargerIdleAction>(); }

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
