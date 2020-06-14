using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerStunAction : CharacterAction
{

public static PlayerStunAction GetInstance() { return ObjectPooling.PopObject<PlayerStunAction>(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.PlayAnim(Owner ,"stun");
        InGameInterface.Instance.GlobalCardState.IsLock = true;
}

public override void UpdateAction()
{
base.UpdateAction();

if(NodeUtil.StateActionMacroByCurrentOrder(Owner ,3))
{
}

else
{

            InGameInterface.Instance.GlobalCardState.IsLock = true;

            if (NodeUtil.StateFinishActionMacro(Owner ,3))
{
}

else
{
}
}
}

public override void FinishAction()
{
base.FinishAction();
        InGameInterface.Instance.GlobalCardState.IsLock = false;
    }
}
