using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerFamilyKillAction : PlayerCardAction
{
    public static PlayerFamilyKillAction GetInstance(CardTable dataTable, TargetData target)
    { return ObjectPooling.PopObject<PlayerFamilyKillAction>().SetData(dataTable, target) as PlayerFamilyKillAction; }

    List<Monster> targetCharacters;
    float damage;

    int currentTargetIndex;

    //0 - pre skill, 1 - on skill, 2 - post skill
    int StateOrder = 0;

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        currentTargetIndex = 0;
        StateOrder = 0;

        AnimUtil.PlayAnim(owner, "effect0");

        targetCharacters = new List<Monster>();

        Monster[] enemys = Object.FindObjectsOfType<Monster>();

        targetCharacters = enemys.ToList().FindAll((c) => c.SystemName == (Target.Target as Monster).SystemName);
        targetCharacters.Sort((c1, c2) => (c1.transform.position - owner.transform.position).magnitude 
        < (c2.transform.position - owner.transform.position).magnitude ? -1 : 1);

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

        if (targetCharacters.Count == 0)
            ReadyToFinish();
        else
        {
            AttackCurrentTarget();
        }

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

    public void AttackCurrentTarget()
    {
        Vector3 velocity = Owner.RenderTrasform.transform.position - targetCharacters[currentTargetIndex].RenderTrasform.transform.position;
        velocity.z = 0f;
        velocity.Normalize();

        Vector3 oPos = Owner.transform.position, tPos = targetCharacters[currentTargetIndex].transform.position;

        float zAngle = Quaternion.FromToRotation(Vector3.right, velocity).eulerAngles.z;

        IsoParticle.CreateParticle("Sliced_Family", targetCharacters[currentTargetIndex].transform.position, zAngle, false , 0.3f);
        targetCharacters[currentTargetIndex].AddState(new CharacterHitState(targetCharacters[currentTargetIndex], damage, 0.1f).Init());
        Owner.NavAgent.Move(targetCharacters[currentTargetIndex].transform.position - Owner.transform.position);

        System.Action nextAction;

        if (currentTargetIndex < targetCharacters.Count - 1)
        {
            TimelineEvents.Add(new TimeLineEvent(ElapsedTime + 0.25f, RotateNextTarget));
        }
        else
        {
            nextAction = ()=> GotoBehindPosition(tPos, oPos);
            nextAction += ReadyToFinish;
            TimelineEvents.Add(new TimeLineEvent(ElapsedTime + 0.2f, nextAction));
        }
    }

    public void RotateNextTarget()
    {
        Vector3 velocity = Owner.RenderTrasform.transform.position - targetCharacters[currentTargetIndex + 1].RenderTrasform.transform.position;

        velocity.z = 0f;
        velocity.Normalize();

        float zAngle = Quaternion.FromToRotation(Vector3.right, velocity).eulerAngles.z;

        IsoParticle.CreateParticle("Round_Family", Owner.transform.position, 0f, false, 0.2f);
        IsoParticle.CreateParticle("Rotate_Family", Owner.transform.position, zAngle, false, 0.1f);

        TimelineEvents.Add(new TimeLineEvent(ElapsedTime + 0.1f, AttackCurrentTarget));
        currentTargetIndex++;
    }
}
