using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleAction : CharacterAction
{
    public static CharacterAction Instance = new PlayerIdleAction();

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        AnimUtil.PlayAnim(owner, "idle");
    }

    public override void UpdateAction()
    {
        base.UpdateAction();

        if(EntityUtil.HitDeadLogicMacro(Owner, "PlayerHitAction", "PlayerDeadAction"))
        {
            return;
        }

        if (PlayerUtil.GetAttackInput())
        {
            Owner.CurrentAction = PlayerAttackAction.Instance;
            return;
        }

        Vector3 velocity = PlayerUtil.GetVelocityInput();

        if (velocity.magnitude > 0.1f)
        {
            Owner.CurrentAction = PlayerMoveAction.Instance;
            return;
        }
    }
}
