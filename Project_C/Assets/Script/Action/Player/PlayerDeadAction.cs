using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



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

        if(AnimUtil.IsLastFrame(Owner))
        {
            SceneManager.LoadScene("Prototype", LoadSceneMode.Single);
            return;
        }
    }
    
    public override void FinishAction()
    {
        base.FinishAction();
    }
}
