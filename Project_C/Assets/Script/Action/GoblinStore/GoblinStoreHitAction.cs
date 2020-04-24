using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinStoreHitAction : CharacterAction
{

public static GoblinStoreHitAction GetInstance() { return new GoblinStoreHitAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.LookPlayer(Owner);
NodeUtil.PlayAnim(Owner ,"hit");
}

public override void UpdateAction()
{
base.UpdateAction();

if(NodeUtil.StateActionMacroByCurrentOrder(Owner ,3))
{
}

else
{

if(NodeUtil.IsLastFrame(Owner))
{
NodeUtil.ChangeAction(Owner ,"GoblinHentaiIdleAction");
}

else
{
}
}
}

public override void FinishAction()
{
base.FinishAction();
}
}
