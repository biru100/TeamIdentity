using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Reflection;

public static class EntityUtil
{
    public static bool StateActionMacro(Character owner, CharacterStateType currentOrder = CharacterStateType.E_Idle)
    {
        string ownerName = owner.GetType().Name;
        int order = CharacterState.CharacterStateActionOrder.FindIndex((s)=>s == currentOrder);
        order = order < 0 ? CharacterState.CharacterStateActionOrder.Count : order;

        for (int i = 0; i < order; ++i)
        {
            CharacterStateType curType = CharacterState.CharacterStateActionOrder[i];
            if (owner.Status.CurrentStates.Contains(curType))
            {
                if(CharacterState.IsCharacterStatePlayingAction[curType])
                    ChangeAction(owner, ownerName + CharacterState.CharacterStateActionName[curType]);
                return true;
            }
        }

        return false;
    }

    public static bool StateFinishActionMacro(Character owner, CharacterStateType currentOrder)
    {
        string ownerName = owner.GetType().Name;

        if (!owner.Status.CurrentStates.Contains(currentOrder))
        {
            ChangeAction(owner, ownerName + "IdleAction");
            return true;
        }

        return false;
    }

    public static bool IsActivateAbility(Character owner, CharacterAbilityType wantToAbility)
    {
        return owner.Status.CurrentAbility.Contains(wantToAbility);
    }

    public static bool IsStun(Character owner)
    {
        return owner.Status.CurrentStates.Contains(CharacterStateType.E_Stun);
    }

    public static bool IsHold(Character owner)
    {
        return owner.Status.CurrentStates.Contains(CharacterStateType.E_Hold);
    }

    public static bool IsSilence(Character owner)
    {
        return owner.Status.CurrentStates.Contains(CharacterStateType.E_Silence);
    }

    public static void ChangeAction(Character owner, string actionName)
    {
        owner.CurrentAction = (CharacterAction)Type.GetType(actionName).GetMethod("GetInstance", BindingFlags.Public | BindingFlags.Static).Invoke(null, null);
    }

    public static void ChangeCardAction(Character owner, string actionName, TargetData target)
    {
        owner.CurrentAction = (CharacterAction)Type.GetType(actionName).GetMethod("GetInstance", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { target });
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

    public static bool GetAttackInput()
    {
        return Input.GetKeyDown(KeyCode.Mouse0) 
            && !EventSystem.current.IsPointerOverGameObject();
    }

    public static bool GetDashInput()
    {
        return Input.GetKeyDown(KeyCode.Space);
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
        return ((owner.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime)
            + Time.deltaTime / owner.Anim.GetCurrentAnimatorStateInfo(0).length) >= 0.9f;
    }
}
