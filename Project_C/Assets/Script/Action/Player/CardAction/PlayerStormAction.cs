using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStormAction : PlayerCardAction
{
    public static PlayerStormAction GetInstance(CardTable dataTable, TargetData target)
    { return ObjectPooling.PopObject<PlayerStormAction>().SetData(dataTable, target) as PlayerStormAction; }

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        AnimUtil.PlayAnim(owner, "buff");
        Owner.AddState(new CharacterState(CharacterStateType.E_SuperArmor, Owner).Init());
        for (int i = 1; i >= DataTable._Parameter[1]; i++)
        {
            TimelineEvents.Add(new TimeLineEvent(0.5f, SendDamage));
        }

    }

    public override void UpdateAction()
    {
        base.UpdateAction();

        if (EntityUtil.StateActionMacro(Owner, CharacterStateType.E_Hold))
        {
            return;
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
        Owner.DeleteState(CharacterStateType.E_SuperArmor);
    }

    public void SendDamage()
    {

        Character[] enemys = Object.FindObjectsOfType<Monster>();
        float damage = PlayerUtil.CalculatingCardPowerValue(DataTable._Parameter[0]);

        if (enemys == null)
            return;

        foreach (var e in enemys)
        {
            if (e == Owner) continue;

            if ((Target.Point -
                e.transform.position).magnitude <= Isometric.IsometricTileSize.x * 3f)
            {
                e.AddState(new CharacterHitState(e, damage, 0.1f).Init());
                e.AddState(new CharacterState(CharacterStateType.E_Stun, e, DataTable._Parameter[1]));
                IsoParticle.CreateParticle("Sliced_Power1", e.transform.position
                    + Vector3.up * Isometric.IsometricTileSize.y * e.EffectOffset,
                    0f);
                IsoParticle.CreateParticle("Sliced_Power2", e.transform.position
                    + Vector3.up * Isometric.IsometricTileSize.y * e.EffectOffset,
                    0f);
            }
        }

        PlayerUtil.ConsumeCardPowerUpStatus();
    }
}
