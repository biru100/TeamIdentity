using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerAssassinAction : PlayerCardAction
{
    public static PlayerAssassinAction GetInstance(CardTable dataTable, TargetData target) { return new PlayerAssassinAction(dataTable, target); }

    List<Monster> targetCharacters;
    float damage;

    int currentTargetIndex;

    //0 - pre skill, 1 - on skill, 2 - post skill
    int StateOrder = 0;

    public PlayerAssassinAction(CardTable dataTable, TargetData target) : base(dataTable, target)
    {
    }

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        AnimUtil.PlayAnim(owner, "effect0");

        damage = PlayerUtil.CalculatingCardPowerValue(DataTable._Parameter[0]);
        Owner.AddState(new CharacterState(CharacterStateType.E_Invincibility, Owner).Init());
    }

    public override void UpdateAction()
    {
        if (StateOrder == 0 && AnimUtil.IsLastFrame(Owner))
        {
            ReadyToAction();
        }
        else if (StateOrder == 2 && AnimUtil.IsLastFrame(Owner))
        {
            Owner.CurrentAction = PlayerIdleAction.GetInstance();
        }

        base.UpdateAction();


    }

    public override void FinishAction()
    {
        base.FinishAction();
        PlayerUtil.ConsumeCardPowerUpStatus();
        Owner.DeleteState(CharacterStateType.E_Invincibility);
    }

    public void ReadyToAction()
    {
        StateOrder = 1;
        Owner.RenderTrasform.GetComponent<SpriteRenderer>().enabled = false;
        Vector3 velocity = Owner.RenderTrasform.transform.position - Target.Target.RenderTrasform.transform.position;
        velocity.z = 0f;
        velocity.Normalize();

        Vector3 oPos = Owner.transform.position, tPos = Target.Target.transform.position;

        float zAngle = Quaternion.FromToRotation(Vector3.right, velocity).eulerAngles.z;

        IsoParticle.CreateParticle("Sliced_Family", Target.Target.transform.position, zAngle, false, 0.3f);
        Target.Target.AddState(new CharacterHitState(Target.Target, damage, 0.1f).Init());
        GotoBehindPosition(Target.Target.transform.position, Owner.transform.position);
        ReadyToFinish();

    }

    public void GotoBehindPosition(Vector3 target, Vector3 player)
    {
        Owner.NavAgent.Move(target - player + (target - player).normalized * Isometric.IsometricTileSize.x * 0.5f);
        Owner.transform.rotation = Quaternion.LookRotation((target - player).normalized);
    }

    public void ReadyToFinish()
    {
        StateOrder = 2;
        Owner.RenderTrasform.GetComponent<SpriteRenderer>().enabled = true;
        AnimUtil.PlayAnim(Owner, "stand_up");
    }
}
