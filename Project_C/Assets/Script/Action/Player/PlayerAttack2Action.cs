using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack2Action : CharacterAction
{
    public static PlayerAttack2Action GetInstance() { return new PlayerAttack2Action(); }

    Vector3 originPos;

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        AnimUtil.PlayAnim(owner, "attack2");
        originPos = Owner.transform.position;
        TimelineEvents.Add(new TimeLineEvent(0.22f, SendDamage));
    }

    public override void UpdateAction()
    {
        base.UpdateAction();

        if (EntityUtil.HitDeadLogicMacro(Owner, "PlayerHitAction", "PlayerDeadAction"))
        {
            return;
        }

        Owner.NavAgent.Move(Owner.transform.forward * Isometric.IsometricTileSize.x * 0.5f * Time.deltaTime);

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

        foreach(var e in enemys)
        {
            if (e == Owner) continue;
            float angle = Mathf.Acos(Vector3.Dot((e.transform.position - Owner.transform.position).normalized, Owner.transform.forward))
                * Mathf.Rad2Deg;
            if ((Owner.transform.position - e.transform.position).magnitude <= Isometric.IsometricTileSize.x * 1.8f &&
                angle < 80f)
            {
                e.AddNotifyEvent(new CharacterNotifyEvent(CharacterNotifyType.E_Damage, 30f));
                IsoParticle.CreateParticle("SlicedParticle1", e.transform.position
                    + new Vector3(0f, Isometric.IsometricTileSize.y * 0.5f, 0f),
                    angle, 0.4f);
                IsoParticle.CreateParticle("SlicedParticle2", e.transform.position
                    + new Vector3(0f, Isometric.IsometricTileSize.y * 0.5f, 0f),
                    angle + 90f, 0.4f);
            }
        }
    }
}
