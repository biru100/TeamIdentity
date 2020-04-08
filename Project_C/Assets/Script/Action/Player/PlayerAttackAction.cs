using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackAction : CharacterAction
{
    public static CharacterAction Instance = new PlayerAttackAction();

    Vector3 originPos;

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        AnimUtil.PlayAnim(owner, "attack");
        originPos = Owner.transform.position;
    }

    public override void UpdateAction()
    {
        base.UpdateAction();

        Owner.NavAgent.Move(Owner.transform.forward * Isometric.IsometricTileSize.x * 0.7f * Time.deltaTime);

        if (AnimUtil.IsLastFrame(Owner))
        {
            Owner.CurrentAction = PlayerIdleAction.Instance;
            return;
        }
    }
}
