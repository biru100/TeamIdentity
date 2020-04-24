using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinDrunkHitAction : CharacterAction
{

public static GoblinDrunkHitAction GetInstance() { return new GoblinDrunkHitAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
TimelineEvents.Add(new TimeLineEvent(0.01f, TimeLine_4));
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
NodeUtil.ChangeAction(Owner ,"GoblinDrunkIdleAction");
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

void TimeLine_4()
{
NodeUtil.ChangeAction(NodeUtil.CreateEntity("GoblinDrunk" ,NodeUtil.VectorAdd(NodeUtil.GetPosition(Owner) ,NodeUtil.CreateVector3(0.4808326f ,0f ,0f))) ,"GoblinDrunkIdleAction");
}
}
