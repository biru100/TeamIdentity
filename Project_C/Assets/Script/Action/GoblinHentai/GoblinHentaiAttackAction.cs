using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinHentaiAttackAction : CharacterAction
{

public static GoblinHentaiAttackAction GetInstance() { return new GoblinHentaiAttackAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
TimelineEvents.Add(new TimeLineEvent(0.01f, TimeLine_4));
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

void TimeLine_4()
{
NodeUtil.TakeDamageToPlayer(10f);
NodeUtil.DrawCard();
}
}
