using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Reflection;

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
        owner.CurrentAction = (CharacterAction)Type.GetType(actionName).GetMethod("GetInstance", BindingFlags.Public | BindingFlags.Static).Invoke(null, null);
    }
}

public static class PlayerUtil
{
    public static float CalculatingCardPowerValue(float basePower)
    {
        return (basePower + PlayerStatus.CurrentStatus.CardPowerSupport) * PlayerStatus.CurrentStatus.CardPowerScale;
    }

    public static void ConsumeCardPowerUpStatus()
    {
        PlayerStatus.CurrentStatus.CardPowerSupport = PlayerStatus.CurrentStatus.BaseCardPowerSupport;
        PlayerStatus.CurrentStatus.CardPowerScale = PlayerStatus.CurrentStatus.BaseCardPowerScale;
    }

    public static void CardInterfaceLogicMacro()
    {
        //CardSlotType type;
        //if(GetCardKeyDown(out type))
        //{
        //    if (!InGameInterface.IsShowCard)
        //    {
        //        PlayerStatus.CurrentStatus.ShowCard(type);
        //    }
        //    else if(InGameInterface.IsShowCard && InGameInterface.ShowSlot == type)
        //    {
        //        PlayerStatus.CurrentStatus.UseCard(type);
        //    }
        //}
        //else if(InGameInterface.IsShowCard && Input.GetKeyDown(KeyCode.C))
        //{
        //    PlayerStatus.CurrentStatus.HideCard(InGameInterface.ShowSlot);
        //}
    }

    public static bool GetCardKeyDown(out CardSlotType type)
    {
        //if(Input.GetKeyDown(KeyCode.A))
        //{
        //    type = CardSlotType.E_A;
        //    return true;
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    type = CardSlotType.E_S;
        //    return true;
        //}
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    type = CardSlotType.E_D;
        //    return true;
        //}
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    type = CardSlotType.E_F;
        //    return true;
        //}

        type = CardSlotType.E_None;
        return false;
    }

    public static bool GetAttackInput()
    {
        return !PlayerStatus.CurrentStatus.IsStun && Input.GetKeyDown(KeyCode.Mouse0) 
            && !EventSystem.current.IsPointerOverGameObject();
    }

    public static bool GetDashInput()
    {
        return !PlayerStatus.CurrentStatus.IsStun && Input.GetKeyDown(KeyCode.C);
    }

    public static Vector3 GetVelocityInput()
    {
        Vector3 velocity = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            velocity += new Vector3(-1f, 0f, 1f);
        }

        if (Input.GetKey(KeyCode.S))
        {
            velocity += new Vector3(1f, 0f, -1f);
        }

        if (Input.GetKey(KeyCode.D))
        {
            velocity += new Vector3(1f, 0f, 1f);
        }

        if (Input.GetKey(KeyCode.A))
        {
            velocity += new Vector3(-1f, 0f, -1f);
        }

        return PlayerStatus.CurrentStatus.IsStun ? Vector3.zero : velocity.normalized;
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

    public static void PlayAnimOneSide(Character owner, string animationName)
    {
        owner.Anim.Play(animationName, 0);
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
        if (owner.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
            return false;

        return ((owner.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime)
            + Time.deltaTime / owner.Anim.GetCurrentAnimatorStateInfo(0).length) >= 0.99f;
    }
}
