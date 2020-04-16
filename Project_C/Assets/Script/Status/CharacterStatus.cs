using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStatus : CharacterStatus
{
    public static PlayerStatus CurrentStatus{ get; set; }

    public int BaseCardPowerScale { get; set; }
    public int CardPowerScale { get; set; }
    public int BaseCardPowerSupport { get; set; }
    public int CardPowerSupport { get; set; }

    public PlayerStatus(Character owner) : base(owner)
    {
        Hp = 100000000000f;
        CurrentHp = 100000000000f;

        CurrentStatus = this;

        BaseCardPowerScale = 1;
        BaseCardPowerSupport = 0;
        CardPowerScale = BaseCardPowerScale;
        CardPowerSupport = BaseCardPowerSupport;
    }

    public override bool SendStatusNotify(CharacterNotifyEvent notify)
    {
        if (notify.Type == CharacterNotifyType.E_Damage)
        {
            Owner.ConsumeNotifyEvent(notify);
            return false;
        }

        return base.SendStatusNotify(notify);
    }
}

public class CharacterStatus
{
    public Character Owner { get; set; }

    public float Damage { get; set; }
    public float CurrentDamage { get; set; }
    public float Armor { get; set; }
    public float CurrentArmor { get; set; }
    public float Hp { get; set; }
    public float CurrentHp { get; set; }
    public float Speed { get; set; }
    public float CurrentSpeed { get; set; }

    public float CurrentStunTime { get; set; }
    public bool IsStun { get; set; }
    public bool IsInvincibility { get; set; }

    public CharacterStatus(Character owner)
    {
        Owner = owner;

        Damage = 10f;
        CurrentDamage = 10f;

        Armor = 0f;
        CurrentArmor = 0f;

        Hp = 100f;
        CurrentHp = 100f;


        Speed = 1.2f;
        CurrentSpeed = 1.2f;
    }

    public virtual void UpdateStatus()
    {
        if (IsStun)
        {
            CurrentStunTime -= Time.deltaTime;
            if (CurrentStunTime <= 0f)
                IsStun = false;
        }
    }

    public virtual bool SendStatusNotify(CharacterNotifyEvent notify)
    {
        if (IsInvincibility)
            return false;

        if (notify.Type == CharacterNotifyType.E_Damage && CurrentHp > 0f)
        {
            CurrentHp = Mathf.Max(CurrentHp - (float)notify.Data, 0f);
            if(CurrentHp <= 0f)
            {
                Owner.ConsumeNotifyEvent(notify);
                Owner.AddNotifyEvent(new CharacterNotifyEvent(CharacterNotifyType.E_Dead, null));
                return false;
            }
        }

        else if (notify.Type == CharacterNotifyType.E_Stun)
        {
            IsStun = true;
            CurrentStunTime = (float)notify.Data;
        }
        else if (notify.Type == CharacterNotifyType.E_Invincibility)
        {
            IsInvincibility = (bool)notify.Data;
        }

        return true;
    }
}
