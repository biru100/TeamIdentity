using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WoodTotemDeadAction : CharacterAction
{

public static WoodTotemDeadAction GetInstance() { return new WoodTotemDeadAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.PlayAnimOneSide(Owner ,"die_135");
}

public override void UpdateAction()
{
base.UpdateAction();

if(NodeUtil.IsLastFrame(Owner))
{
NodeUtil.DestroyEntity(Owner);
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
