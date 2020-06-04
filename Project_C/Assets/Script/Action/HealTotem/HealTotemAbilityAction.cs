using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HealTotemAbilityAction : CharacterAction
{

public static HealTotemAbilityAction GetInstance() { return ObjectPooling.PopObject<HealTotemAbilityAction>(); }

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
