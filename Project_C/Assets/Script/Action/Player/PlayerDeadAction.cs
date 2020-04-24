using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerDeadAction : CharacterAction
{
    public static PlayerDeadAction GetInstance() { return new PlayerDeadAction(); }

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        NodeUtil.PlayAnim(Owner ,"die");
    }
    
    public override void UpdateAction()
    {
        base.UpdateAction();
    }
    
    public override void FinishAction()
    {
        base.FinishAction();
    }
}
