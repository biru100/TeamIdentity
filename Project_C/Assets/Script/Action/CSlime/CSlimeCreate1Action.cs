using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CSlimeCreate1Action : CharacterAction
{

public static CSlimeCreate1Action GetInstance() { return new CSlimeCreate1Action(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.PlayAnimOneSide(Owner ,"create_0");
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
