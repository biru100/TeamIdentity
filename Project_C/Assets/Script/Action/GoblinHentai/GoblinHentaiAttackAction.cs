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

if(NodeUtil.PlayerInSight(Owner ,1f ,65f))
{
NodeUtil.TakeDamageToPlayer(10f);

if(NodeUtil.IsActivateAbility(Owner ,214))
{
NodeUtil.DrawCard();
}

else
{
}
}

else
{
}
}
}
