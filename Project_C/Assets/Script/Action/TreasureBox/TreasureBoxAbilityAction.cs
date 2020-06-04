using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TreasureBoxAbilityAction : CharacterAction
{

public static TreasureBoxAbilityAction GetInstance() { return ObjectPooling.PopObject<TreasureBoxAbilityAction>(); }

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
