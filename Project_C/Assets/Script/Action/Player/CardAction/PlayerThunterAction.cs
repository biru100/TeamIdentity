using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThunterAction : PlayerCardAction
{
    public static PlayerThunterAction GetInstance(CardTable dataTable, TargetData target)
    { return ObjectPooling.PopObject<PlayerThunterAction>().SetData(dataTable, target) as PlayerThunterAction; }

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        AnimUtil.PlayAnim(owner, "buff");
        TimelineEvents.Add(new TimeLineEvent(0.22f, SendDamage));
        Owner.AddState(new CharacterState(CharacterStateType.E_SuperArmor, Owner).Init());
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

        Character[] enemys = Object.FindObjectsOfType<Character>();
        float damage = PlayerUtil.CalculatingCardPowerValue(DataTable._Parameter[0]);

        if (enemys == null)
            return;

        foreach(var e in enemys)
        {
            if (e == Owner) continue;

            if ((Target.Point - 
                e.transform.position).magnitude <= Isometric.IsometricTileSize.x * 1.5f)
            {
                e.AddState(new CharacterHitState(e, damage, 0.1f).Init());
                e.AddState(new CharacterState(CharacterStateType.E_Stun, e, DataTable._Parameter[1]));
                IsoParticle.CreateParticle("Sliced_Power1", e.transform.position
                    + new Vector3(0f, Isometric.IsometricTileSize.y * 0.5f, 0f), 0f
                    );
                IsoParticle.CreateParticle("Sliced_Power2", e.transform.position
                    + new Vector3(0f, Isometric.IsometricTileSize.y * 0.5f, 0f),
                    0f);
            }
        }

        PlayerUtil.ConsumeCardPowerUpStatus();
    }
}
