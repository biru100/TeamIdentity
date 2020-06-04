using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CardChargerAbilityAction : CharacterAction
{

public static CardChargerAbilityAction GetInstance() { return ObjectPooling.PopObject<CardChargerAbilityAction>(); }

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
