using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CSlimeHitAction : CharacterAction
{

public static CSlimeHitAction GetInstance() { return new CSlimeHitAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.LookPlayer(Owner);
NodeUtil.PlayAnim(Owner ,"hit");
}

public override void UpdateAction()
{
base.UpdateAction();

if(NodeUtil.IsLastFrame(Owner))
{
NodeUtil.ChangeAction(Owner ,"CSlimeIdleAction");
}

else
{
}
}

public override void FinishAction()
{
base.FinishAction();
}
}
