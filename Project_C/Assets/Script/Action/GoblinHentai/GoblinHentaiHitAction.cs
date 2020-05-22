using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinHentaiHitAction : CharacterAction
{

    public static GoblinHentaiHitAction GetInstance() { return new GoblinHentaiHitAction(); }

    //히트 스타트 액션
    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        NodeUtil.LookPlayer(Owner); 
        NodeUtil.PlayAnim(Owner, "hit");
    }

    //히트 업데이트 액션
    public override void UpdateAction()
    {
        base.UpdateAction();

        if (NodeUtil.StateActionMacroByCurrentOrder(Owner, 6))
        {
        }

        else
        {

            if (NodeUtil.IsLastFrame(Owner))
            {
                NodeUtil.ChangeAction(Owner, "GoblinHentaiIdleAction");
            }

            else
            {
            }
        }
    }

    // 히트 피니시 액션
    public override void FinishAction()
    {
        base.FinishAction();
    }
}
