using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterStateType
{
    E_Idle, // 기본상태
    E_Taunt, // 도발
    E_TauntInvincibility, // 도발 무적
    E_Invincibility, // 무적
    E_Silence, // 침묵
    E_Hold, // 속박
    E_Stun, // 기절
    E_Slow, // 슬로우
    E_Hit, // 피격
    E_SuperArmor, // 피격 무시
    E_PreventMovement, // 이동 불가 능력
    E_PreventAttack, //공격 불가 능력
    E_Dead
}

public class CharacterMosterBaseState : CharacterState
{
    public CharacterMosterBaseState(CharacterStateType type, Character owner) 
        : base(type, owner)
    {
    }

    public override bool UpdateState()
    {
        Status.CurrentStates.Add(StateType);
        return true;
    }
}

public class CharacterTauntState : CharacterState
{
    //방 한테 몬스터 정보 불러옴 - 도발을 제외한 몬스터에게 도발무적 상태를 제공 
    public CharacterTauntState(Character owner, float lifeTime = -1)
        : base(CharacterStateType.E_Taunt, owner, lifeTime)
    {

    }

    public override bool UpdateState()
    {
        bool retVal = base.UpdateState();
        if (retVal == false)
            return false;

        Status.CurrentStates.Add(CharacterStateType.E_Taunt);
        return true;
    }
}

public class CharacterTauntInvincibilityState : CharacterState
{
    public CharacterTauntInvincibilityState(Character owner, float lifeTime = -1) 
        : base(CharacterStateType.E_TauntInvincibility, owner, lifeTime)
    {

    }

    public override bool UpdateState()
    {
        bool retVal = base.UpdateState();
        if (retVal == false)
            return false;

        Status.CurrentStates.Add(CharacterStateType.E_TauntInvincibility);
        return true;
    }
}

public class CharacterInvincibilityState : CharacterState
{
    public CharacterInvincibilityState(Character owner, float lifeTime = -1)
        : base(CharacterStateType.E_Invincibility, owner, lifeTime)
    {

    }

    public override bool UpdateState()
    {
        bool retVal = base.UpdateState();
        if (retVal == false)
            return false;

        Status.CurrentStates.Add(CharacterStateType.E_Invincibility);
        return true;
    }
}

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
        || s != CharacterStateType.E_Dead);
        Status.CurrentStates.Add(CharacterStateType.E_Silence);

        return true;
    }
}

public class CharacterHoldState : CharacterState
{
    public CharacterHoldState(Character owner, float lifeTime = -1)
        : base(CharacterStateType.E_Hold, owner, lifeTime)
    {

    }

    public override bool UpdateState()
    {
        bool retVal = base.UpdateState();
        if (retVal == false)
            return false;

        Status.CurrentStates.Add(CharacterStateType.E_Hold);

        return true;
    }
}

public class CharacterStunState : CharacterState
{
    public CharacterStunState(Character owner, float lifeTime = -1)
        : base(CharacterStateType.E_Stun, owner, lifeTime)
    { 

    }

    public override bool UpdateState()
    {
        bool retVal = base.UpdateState();
        if (retVal == false)
            return false;

        Status.CurrentStates.Add(CharacterStateType.E_Stun);

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
            Owner.AddState(new CharacterDeadState(Owner).Init());
        }
        return this;
    }

    public override bool UpdateState()
    {
        bool retVal = base.UpdateState();
        if (retVal == false)
            return false;

        if (Status.CurrentStates.FindIndex((s)=> s == CharacterStateType.E_Stun 
            || s == CharacterStateType.E_Hold
            || s == CharacterStateType.E_Dead
            || s == CharacterStateType.E_SuperArmor
            || s == CharacterStateType.E_TauntInvincibility
            || s == CharacterStateType.E_Invincibility) < 0)
            Status.CurrentStates.Add(CharacterStateType.E_Hit);

        return true;
    }
}

public class CharacterDeadState : CharacterState
{
    public CharacterDeadState(Character owner) : base(CharacterStateType.E_Dead, owner)
    {

    }

    public override bool UpdateState()
    {
        Status.CurrentStates.Clear();
        Status.CurrentStates.Add(CharacterStateType.E_Dead);
        return true;
    }
}

public class CharacterSuperArmorState : CharacterState
{
    public CharacterSuperArmorState(Character owner) : base(CharacterStateType.E_SuperArmor, owner)
    {
        
    }

    public override bool UpdateState()
    {
        Status.CurrentStates.Add(CharacterStateType.E_SuperArmor);
        return true;
    }
}

public class CharacterState
{
    public static readonly List<CharacterStateType> CharacterStateActionOrder = new List<CharacterStateType>()
    {
        CharacterStateType.E_Dead,
        CharacterStateType.E_Stun,
        CharacterStateType.E_Hold,
        CharacterStateType.E_Hit
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
    public int StateActionOrder { get; protected set; }

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
        return true;
    }

    public virtual void ForcedDeleteState()
    {
        Owner.DeleteState(this);
    }
}
