using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EntityUtil
{
    public static bool HitDeadLogicMacro(Character owner, string hitActionName, string deadActionName)
    {
        if (GetDeadNotify(owner))
        {
            ChangeAction(owner, deadActionName);
            return true;
        }

        if (GetDamageNotify(owner))
        {
            ChangeAction(owner, hitActionName);
            return true;
        }

        return false;
    }

    public static bool DeadLogicMacro(Character owner, string deadActionName)
    {
        if (GetDeadNotify(owner))
        {
            ChangeAction(owner, deadActionName);
            return true;
        }
        return false;
    }

    public static bool GetDamageNotify(Character owner)
    {
        return owner.GetNotifyEvents().Find((n) => n.Type == CharacterNotifyType.E_Damage) != null;
    }

    public static bool GetDeadNotify(Character owner)
    {
        return owner.GetNotifyEvents().Find((n) => n.Type == CharacterNotifyType.E_Dead) != null;
    }

    public static void ChangeAction(Character owner, string actionName)
    {
        owner.CurrentAction = (CharacterAction)Type.GetType(actionName).GetField("Instance").GetValue(null);
    }
}

public static class PlayerUtil
{

    public static bool GetAttackInput()
    {
        return Input.GetKeyDown(KeyCode.X);
    }

    public static bool GetDashInput()
    {
        return Input.GetKeyDown(KeyCode.C);
    }

    public static Vector3 GetVelocityInput()
    {
        Vector3 velocity = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            velocity += new Vector3(-1f, 0f, 1f);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            velocity += new Vector3(1f, 0f, -1f);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            velocity += new Vector3(1f, 0f, 1f);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            velocity += new Vector3(-1f, 0f, -1f);
        }

        return velocity.normalized;
    }
}

public static class AnimUtil
{
    public static int GetRenderAngle(Quaternion rotation)
    {
        return (int)(Mathf.Round(rotation.eulerAngles.y / 45f) * 45f);
    }

    public static void PlayAnim(Character owner, string animationName)
    {
        owner.Anim.Play(animationName + "_" + GetRenderAngle(owner.transform.rotation), 0);
    }

    //use movement
    public static void RotationAnim(Character owner, string animationName)
    {
        string stateName = animationName + "_" + GetRenderAngle(owner.transform.rotation);
        if(Animator.StringToHash(stateName) != owner.Anim.GetCurrentAnimatorStateInfo(0).shortNameHash)
            owner.Anim.Play(stateName, 0, owner.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }

    public static float GetAnimNormalizedTime(Character owner)
    {
        return owner.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    public static bool IsLastFrame(Character owner)
    {
        return ((owner.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime)
            + Time.deltaTime / owner.Anim.GetCurrentAnimatorStateInfo(0).length) >= 0.99f;
    }
}
