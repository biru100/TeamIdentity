using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinDrunkCreateAction : CharacterAction
{

    public static GoblinDrunkCreateAction GetInstance() { return new GoblinDrunkCreateAction(); }

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        NodeUtil.PlayAnim(Owner, "hit");
        Owner.AddState(new CharacterState(CharacterStateType.E_Invincibility, Owner).Init());
    }

    public override void UpdateAction()
    {
        base.UpdateAction();
        if (NodeUtil.IsLastFrame(Owner))
        {
            NodeUtil.ChangeAction(Owner, "GoblinDrunkIdleAction");
        }
    }

    public override void FinishAction()
    {
        base.FinishAction();
        Owner.DeleteState(CharacterStateType.E_Invincibility);
    }
}
