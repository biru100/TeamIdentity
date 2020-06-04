using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CardPresentAbilityAction : CharacterAction
{

public static CardPresentAbilityAction GetInstance() { return ObjectPooling.PopObject<CardPresentAbilityAction>(); }

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
