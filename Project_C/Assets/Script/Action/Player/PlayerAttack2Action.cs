using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack2Action : CharacterAction
{
    public static PlayerAttack2Action GetInstance() { return ObjectPooling.PopObject<PlayerAttack2Action>(); }


    MovementSetController movementAnimController;

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        AnimUtil.PlayAnim(owner, "attack2");
        TimelineEvents.Add(new TimeLineEvent(0f, SendDamage));

        movementAnimController = MovementSetController.GetMovementSetByAngle(owner.transform.rotation, "MovementList/Attack3Movement");
    }

    public override void UpdateAction()
    {
        base.UpdateAction();

        if (EntityUtil.StateActionMacro(Owner))
        {
            return;
        }

        Vector3 offset;
        if (movementAnimController.GetMovementData(Time.deltaTime, out offset))
        {
            Owner.NavAgent.Move(offset);
        }

        if (AnimUtil.IsLastFrame(Owner))
        {
            Owner.CurrentAction = PlayerIdleAction.GetInstance();
            return;

        }
    }

    public override void FinishAction()
    {
        base.FinishAction();
    }

    public void SendDamage()
    {

        Character[] enemys = Object.FindObjectsOfType<Character>();

        if (enemys == null)
            return;

        bool isValidAttack = false;

        foreach (var e in enemys)
        {
            if (e == Owner) continue;
            Vector3 direction = e.RenderTrasform.GetIsometricPosition() - Owner.RenderTrasform.GetIsometricPosition();
            direction.z = 0f;

            float angle = Quaternion.FromToRotation(Vector3.right, direction.normalized).eulerAngles.z;
            if ((Owner.transform.position - e.transform.position).magnitude <= Isometric.IsometricTileSize.x * 2f &&
                Mathf.Acos(Vector3.Dot((e.transform.position - Owner.transform.position).normalized, Owner.transform.forward)) * Mathf.Rad2Deg < 85f)
            {
                isValidAttack = true;
                e.AddState(new CharacterHitState(e, Owner.Status.CurrentDamage, 0.1f).Init());

                angle -= 12f;

                IsoParticle.CreateParticle("Sliced1", e.transform.position
                    + Vector3.up * Isometric.IsometricTileSize.y * e.EffectOffset,
                    angle);
                IsoParticle.CreateParticle("Sliced2", e.transform.position
                    + Vector3.up * Isometric.IsometricTileSize.y * e.EffectOffset,
                    angle + 90f);
            }
        }

        if (isValidAttack)
            CameraManager.PlayAnim("attack2", AnimUtil.GetRenderAngle(Owner.transform.rotation));
    }
}
