using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WoodTotemIdleAction : CharacterAction
{

public static WoodTotemIdleAction GetInstance() { return new WoodTotemIdleAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
TimelineEvents.Add(new TimeLineEvent(NodeUtil.Random(1f ,2.5f), TimeLine_3));
NodeUtil.PlayAnimOneSide(Owner ,"idle_135");
}

public override void UpdateAction()
{
base.UpdateAction();

if(NodeUtil.HitDeadLogicMacro(Owner ,"WoodTotemHitAction" ,"WoodTotemDeadAction"))
{
}

else
{
}
}

public override void FinishAction()
{
base.FinishAction();
}

void TimeLine_3()
{
NodeUtil.ChangeAction(Owner ,"WoodTotemSpellAction");
}
}
