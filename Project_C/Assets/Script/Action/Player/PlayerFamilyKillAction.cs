using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerFamilyKillAction : CharacterAction
{
    public static PlayerFamilyKillAction GetInstance() { return new PlayerFamilyKillAction(); }

    List<Character> targetCharacters;
    float damage;

    int currentTargetIndex;

    //0 - pre skill, 1 - on skill, 2 - post skill
    int StateOrder = 0;

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        AnimUtil.PlayAnim(owner, "effect0");

        targetCharacters = new List<Character>();

        Character[] enemys = Object.FindObjectsOfType<Character>();

        Character target = null;
        float shortDistance = 99999f;

        foreach (var e in enemys)
        {
            if (e == Owner) continue;

            float angle = Mathf.Acos(Vector3.Dot((e.transform.position - Owner.transform.position).normalized, Owner.transform.forward))
                * Mathf.Rad2Deg;

            float distance = (Owner.transform.position - e.transform.position).magnitude;
            if (distance <= Isometric.IsometricTileSize.x * 1.8f &&
                angle < 80f)
            {
                if(distance < shortDistance)
                {
                    target = e;
                    shortDistance = distance;
                }
            }
        }

        if(target == null)
        {
            return;
        }

        targetCharacters = enemys.ToList().FindAll((c) => c.GetType() == target.GetType());
        targetCharacters.Sort((c1, c2) => (c1.transform.position - owner.transform.position).magnitude 
        < (c2.transform.position - owner.transform.position).magnitude ? -1 : 1);



        damage = PlayerUtil.CalculatingCardPowerValue(50f);
        Owner.AddNotifyEvent(new CharacterNotifyEvent(CharacterNotifyType.E_Invincibility, true));
    }

    public override void UpdateAction()
    {
        base.UpdateAction();

        if (StateOrder == 0 && AnimUtil.IsLastFrame(Owner))
        {
            ReadyToAction();
        }
        else if(StateOrder == 1)
        {
            if (targetCharacters.Count == 0)
                ReadyToFinish();
            else
            {
                AttackCurrentTarget();
            }
        }
        else if (StateOrder == 2 && AnimUtil.IsLastFrame(Owner))
        {
            Owner.CurrentAction = PlayerIdleAction.GetInstance();
        }

    }

    public override void FinishAction()
    {
        base.FinishAction();
        PlayerUtil.ConsumeCardPowerUpStatus();
        Owner.AddNotifyEvent(new CharacterNotifyEvent(CharacterNotifyType.E_Invincibility, false));
    }

    public void ReadyToAction()
    {
        StateOrder = 1;
        Owner.RenderTrasform.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void GotoBehindPosition(Vector3 target, Vector3 player)
    {
         Owner.NavAgent.Move(target - player + (target - player).normalized * Isometric.IsometricTileSize.x);
    }

    public void ReadyToFinish()
    {
        StateOrder = 2;
        Owner.RenderTrasform.GetComponent<SpriteRenderer>().enabled = true;
        AnimUtil.PlayAnim(Owner, "stand_up");
    }

    public void AttackCurrentTarget()
    {
        Vector3 velocity = targetCharacters[currentTargetIndex].RenderTrasform.transform.position - Owner.RenderTrasform.transform.position;
        velocity.z = 0f;
        velocity.Normalize();

        Vector3 oPos = Owner.transform.position, tPos = targetCharacters[currentTargetIndex].transform.position;

        float zAngle = Quaternion.FromToRotation(Vector3.right, velocity).eulerAngles.z;

        IsoParticle.CreateParticle("Slice_Family", targetCharacters[currentTargetIndex].transform.position, zAngle);
        targetCharacters[currentTargetIndex].AddNotifyEvent(new CharacterNotifyEvent(CharacterNotifyType.E_Damage, damage));
        Owner.NavAgent.Move(targetCharacters[currentTargetIndex].transform.position - Owner.transform.position);

        System.Action nextAction;

        if (currentTargetIndex < targetCharacters.Count - 1)
        {
            TimelineEvents.Add(new TimeLineEvent(ElapsedTime + 0.3f, RotateNextTarget));
        }
        else
        {
            nextAction = ()=> GotoBehindPosition(tPos, oPos);
            nextAction += ReadyToFinish;
            TimelineEvents.Add(new TimeLineEvent(ElapsedTime + 0.3f, nextAction));
        }
    }

    public void RotateNextTarget()
    {
        Vector3 velocity = targetCharacters[currentTargetIndex + 1].RenderTrasform.transform.position - Owner.RenderTrasform.transform.position;

        velocity.z = 0f;
        velocity.Normalize();

        float zAngle = Quaternion.FromToRotation(Vector3.right, velocity).eulerAngles.z;

        IsoParticle.CreateParticle("Rotate_Family", Owner.transform.position, zAngle);

        TimelineEvents.Add(new TimeLineEvent(ElapsedTime + 0.12f, AttackCurrentTarget));
        currentTargetIndex++;
    }
}
