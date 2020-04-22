using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WoodTotemSpellAction : CharacterAction
{
    public static WoodTotemSpellAction GetInstance() { return new WoodTotemSpellAction(); }

    IsoHighLight highlight;
    bool isFinish = false;

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        highlight = IsoParticle.CreateParticle("HighLight", Player.CurrentPlayer.transform.position + Vector3.down * Isometric.IsometricGridSize, 0f, false, 4f) as IsoHighLight;
        highlight.IsMove = true;
        highlight.Target = Player.CurrentPlayer.transform;
        highlight.AfterHighLightLogic += CalculateStun;
    }

    public override void UpdateAction()
    {
        base.UpdateAction();

        if(NodeUtil.StateActionMacro(Owner))
        {
            return;
        }
    }

    public override void FinishAction()
    {
        base.FinishAction();
        if (!isFinish)
            UnityEngine.Object.Destroy(highlight.gameObject);
    }

    public void CalculateStun(Vector3 position)
    {
        isFinish = true;
        if ((Player.CurrentPlayer.transform.position - position).magnitude < 1.2 * Isometric.IsometricTileSize.x)
        {
            Player.CurrentPlayer.AddState(new CharacterHoldState(Player.CurrentPlayer, 1f));
            Player.CurrentPlayer.AddState(new CharacterHitState(Player.CurrentPlayer, 20f, 0.1f));
        }

        IsoParticle.CreateParticle("ExplosionHighlight", position, 0f);
        IsoParticle.CreateParticle("WoodHold", position, 0f);

        Owner.CurrentAction = WoodTotemIdleAction.GetInstance();
    }
}
