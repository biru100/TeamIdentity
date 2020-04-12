using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackAction : CharacterAction
{
    protected static int AttackCount = 0;
    public static CharacterAction Instance = new PlayerAttackAction();

    Vector3 originPos;

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        AnimUtil.PlayAnim(owner, "attack" + AttackCount);
        originPos = Owner.transform.position;
        TimelineEvents.Add(new TimeLineEvent(0.25f, SendDamage));
    }

    public override void UpdateAction()
    {
        base.UpdateAction();

        if (EntityUtil.HitDeadLogicMacro(Owner, "PlayerHitAction", "PlayerDeadAction"))
        {
            return;
        }

        Owner.NavAgent.Move(Owner.transform.forward * Isometric.IsometricTileSize.x * 0.7f * Time.deltaTime);

        if (AnimUtil.IsLastFrame(Owner))
        {
            Owner.CurrentAction = PlayerIdleAction.Instance;
            return;
        }
    }

    public override void FinishAction()
    {
        base.FinishAction();
        AttackCount = (AttackCount + 1) % 3;
    }

    public void SendDamage()
    {

        Enemy[] enemys = Object.FindObjectsOfType<Enemy>();

        if (enemys == null)
            return;

        foreach(var e in enemys)
        {
            if((Owner.transform.position - e.transform.position).magnitude <= Isometric.IsometricTileSize.x * 1.5f &&
                Vector3.Dot((e.transform.position - Owner.transform.position).normalized, Owner.transform.forward) < Mathf.Deg2Rad * 90f)
            {
                e.AddNotifyEvent(new CharacterNotifyEvent(CharacterNotifyType.E_Damage, 0f));
                IsoParticle.CreateParticle("SlicedParticle1", e.transform.position
                    + new Vector3(0f, Isometric.IsometricTileSize.y * 0.5f, 0f),
                    0f, 0.4f);
                IsoParticle.CreateParticle("SlicedParticle2", e.transform.position
                    + new Vector3(0f, Isometric.IsometricTileSize.y * 0.5f, 0f),
                    90f, 0.4f);
            }
        }
    }
}
