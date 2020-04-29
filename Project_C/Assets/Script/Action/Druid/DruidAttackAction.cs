using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DruidAttackAction : CharacterAction
{

public static DruidAttackAction GetInstance() { return new DruidAttackAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
TimelineEvents.Add(new TimeLineEvent(0.5f, TimeLine_4));
NodeUtil.PlayAnim(Owner ,"attack");
}

public override void UpdateAction()
{
base.UpdateAction();

if(NodeUtil.StateActionMacro(Owner))
{
}

else
{

if(NodeUtil.IsLastFrame(Owner))
{
NodeUtil.ChangeAction(Owner ,"DruidIdleAction");
}

else
{
NodeUtil.LookPlayer(Owner);
}
}
}

public override void FinishAction()
{
base.FinishAction();
}

void TimeLine_4()
{

if(false)
{
}

else
{
}
}
}
