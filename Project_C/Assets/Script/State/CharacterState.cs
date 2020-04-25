﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//public enum CharacterStateType
//{
//    E_Idle, // 기본상태
//    E_TauntInvincibility, // 도발 무적
//    E_Invincibility, // 무적
//    E_Silence, // 침묵
//    E_Hold, // 속박
//    E_Stun, // 기절
//    E_Slow, // 슬로우
//    E_Hit, // 피격
//    E_SuperArmor, // 피격 무시
//    E_Dead
//}

public class CharacterSilenceState : CharacterState
{
    public CharacterSilenceState(Character owner, float lifeTime = -1)
        : base(CharacterStateType.E_Silence, owner, lifeTime)
    {
        
    }

    public override bool UpdateState()
    {
        bool retVal = base.UpdateState();
        if (retVal == false)
            return false;

        Status.CurrentStates.RemoveAll((s) => s != CharacterStateType.E_TauntInvincibility 
        || s != CharacterStateType.E_Stun
        || s != CharacterStateType.E_Hold
        || s != CharacterStateType.E_Dead
        || s != CharacterStateType.E_Silence);

        return true;
    }
}

public class CharacterHitState : CharacterState
{
    public float Damage { get; set; }

    public CharacterHitState(Character owner, float damage, float lifeTime = -1)
        : base(CharacterStateType.E_Hit, owner, lifeTime)
    {
        Damage = damage;
    }

    public override CharacterState Init()
    {
        if (Status.CurrentStates.Contains(CharacterStateType.E_TauntInvincibility) || Status.CurrentStates.Contains(CharacterStateType.E_Invincibility))
            return this;

        Status.CurrentHp -= Damage;
        if(Status.CurrentHp <= 0f)
        {
            Status.CurrentHp = 0f;
            Owner.AddState(new CharacterState(CharacterStateType.E_Dead, Owner).Init());
        }
        return this;
    }
}

public class CharacterIncreaseDamageState : CharacterState
{
    public float IncreaseDamage { get; set; }

    public CharacterIncreaseDamageState(Character owner, float increaseDamage, float lifeTime = -1)
        : base(CharacterStateType.E_IncreaseDamage, owner, lifeTime)
    {
        IncreaseDamage = increaseDamage;
    }

    public override bool UpdateState()
    {
        bool retVal = base.UpdateState();
        if (retVal == false)
            return false;

        Status.CurrentDamage += IncreaseDamage;

        return true;
    }
}
public class CharacterDecreaseDamageState : CharacterState
{
    public float IncreaseDamage { get; set; }

    public CharacterDecreaseDamageState(Character owner, float increaseDamage, float lifeTime = -1)
        : base(CharacterStateType.E_IncreaseDamage, owner, lifeTime)
    {
        IncreaseDamage = increaseDamage;
    }

    public override bool UpdateState()
    {
        bool retVal = base.UpdateState();
        if (retVal == false)
            return false;

        Status.CurrentDamage += IncreaseDamage;

        return true;
    }
}


public class CharacterState
{
    public static readonly Dictionary<CharacterStateType, Func<Character, float, CharacterState>> CharacterStateBuilderSet = new Dictionary<CharacterStateType, Func<Character, float, CharacterState>>()
    {
        {CharacterStateType.E_Idle,              (c, t)=> new CharacterState(CharacterStateType.E_Idle, c, t).Init()},
        {CharacterStateType.E_TauntInvincibility,(c, t)=> new CharacterState(CharacterStateType.E_TauntInvincibility, c, t).Init()},
        {CharacterStateType.E_Invincibility,     (c, t)=> new CharacterState(CharacterStateType.E_Invincibility, c, t).Init()},
        {CharacterStateType.E_Silence,           (c, t)=> new CharacterSilenceState( c, t).Init()},
        {CharacterStateType.E_Hold,              (c, t)=> new CharacterState(CharacterStateType.E_Hold, c, t).Init()},
        {CharacterStateType.E_Stun,              (c, t)=> new CharacterState(CharacterStateType.E_Stun, c, t).Init()},
        {CharacterStateType.E_Slow,              (c, t)=> new CharacterState(CharacterStateType.E_Slow, c, t).Init()},
        {CharacterStateType.E_Hit,               (c, t)=> new CharacterHitState(c, t).Init()},
        {CharacterStateType.E_SuperArmor,        (c, t)=> new CharacterState(CharacterStateType.E_SuperArmor, c, t).Init()},
        {CharacterStateType.E_Dead,              (c, t)=> new CharacterState(CharacterStateType.E_Dead, c, t).Init()}
    };

    public static readonly List<CharacterStateType> CharacterStateActionOrder = new List<CharacterStateType>()
    {
        CharacterStateType.E_TauntInvincibility,
        CharacterStateType.E_Invincibility,
        CharacterStateType.E_Dead,
        CharacterStateType.E_Stun,
        CharacterStateType.E_Hold,
        CharacterStateType.E_SuperArmor,
        CharacterStateType.E_Hit
    };

    public static readonly Dictionary<CharacterStateType, bool> IsCharacterStatePlayingAction = new Dictionary<CharacterStateType, bool>()
    {
        { CharacterStateType.E_TauntInvincibility, false },
        { CharacterStateType.E_Invincibility, false },
        { CharacterStateType.E_Dead, true },
        { CharacterStateType.E_Stun, true },
        { CharacterStateType.E_Hold, true },
        { CharacterStateType.E_SuperArmor, false },
        { CharacterStateType.E_Hit, true }
    };

    public static readonly Dictionary<CharacterStateType, string> CharacterStateActionName = new Dictionary<CharacterStateType, string>()
    {
        { CharacterStateType.E_Dead, "DeadAction" },
        { CharacterStateType.E_Stun, "StunAction" },
        { CharacterStateType.E_Hold, "HoldAction" },
        { CharacterStateType.E_Hit, "HitAction" }
    };

    public CharacterStateType  StateType { get; protected set; }
    public Character Owner { get; protected set; }
    public CharacterStatus Status { get; protected set; }
    public float StateLifeTime { get; protected set; }
    protected float ElapsedTime { get; set; }

    public CharacterState(CharacterStateType type, Character owner, float lifeTime = -1f)
    {
        Status = owner.Status;
        Owner = owner;
        StateType = type;
        StateLifeTime = lifeTime;
        ElapsedTime = 0f;
    }

    public virtual CharacterState Init()
    {
        return this;
    }

    public virtual bool UpdateState()
    {
        ElapsedTime += Time.deltaTime;
        if(StateLifeTime > 0f && ElapsedTime >= StateLifeTime)
        {
            Owner.DeleteState(this);
            return false;
        }
        Status.CurrentStates.Add(CharacterStateType.E_Dead);
        return true;
    }

    public virtual void ForcedDeleteState()
    {
        Owner.DeleteState(this);
    }
}
