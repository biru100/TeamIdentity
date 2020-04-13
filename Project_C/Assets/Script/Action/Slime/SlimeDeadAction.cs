using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SlimeDeadAction : CharacterAction
{

public static SlimeDeadAction GetInstance() { return new SlimeDeadAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.PlayAnim(Owner ,"die");
}

public override void UpdateAction()
{
base.UpdateAction();

if(NodeUtil.IsLastFrame(Owner))
{
NodeUtil.ChangeAction(NodeUtil.CreateEntity("CSlime" ,NodeUtil.VectorAdd(NodeUtil.GetPosition(Owner) ,NodeUtil.CreateVector3(0.04f ,0f ,0.04f))) ,"CSlimeCreate1Action");
NodeUtil.ChangeAction(NodeUtil.CreateEntity("CSlime" ,NodeUtil.VectorMinus(NodeUtil.GetPosition(Owner) ,NodeUtil.CreateVector3(0.04f ,0f ,0.04f))) ,"CSlimeCreate2Action");
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
