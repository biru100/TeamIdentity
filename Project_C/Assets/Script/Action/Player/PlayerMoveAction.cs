using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveAction : CharacterAction
{
    public static PlayerMoveAction GetInstance() { return ObjectPooling.PopObject<PlayerMoveAction>(); }

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        AnimUtil.PlayAnim(owner, "run");
    }

    public override void UpdateAction()
    {
        base.UpdateAction();

        if (EntityUtil.StateActionMacro(Owner))
        {
            return;
        }

        PlayerUtil.CallInteractionInput(Owner);

        if (PlayerUtil.GetAttackInput())
        {
            Owner.CurrentAction = PlayerAttackAction.GetInstance();
            return;
        }

        Vector3 velocity = PlayerUtil.GetVelocityInput();

        if (velocity.magnitude < 0.1f)
        {
            Owner.CurrentAction = PlayerIdleAction.GetInstance();
            return;
        }

        Owner.transform.rotation = Quaternion.LookRotation(velocity, Vector3.up);
        Owner.NavAgent.Move(velocity * Owner.Status.CurrentSpeed * Time.deltaTime);

        AnimUtil.RotationAnim(Owner, "run");
    }
}
