﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStatus : CharacterStatus
{
    public static PlayerStatus CurrentStatus{ get; set; }

    public int CurrentManaCost { get; set; }

    public int BaseCardPowerSupport { get; set; }
    public int CardPowerSupport { get; set; }

    public PlayerStatus(Character owner) : base(owner)
    {
        Hp = 150f;
        CurrentHp = 150f;

        CurrentStatus = this;

        BaseCardPowerSupport = 0;
        CardPowerSupport = BaseCardPowerSupport;
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

    public List<CharacterStateType> IgnoreStateList { get; set; }

    public CharacterStateType BaseState { get; set; }
    public List<CharacterStateType> CurrentStates { get; set; }
    public List<CharacterAbilityType> CurrentAbility { get; set; }

    public CharacterStatus(Character owner)
    {
        Owner = owner;

        Damage = 10f;
        CurrentDamage = 10f;

        Armor = 0f;
        CurrentArmor = 0f;

        Hp = 100f;
        CurrentHp = 100f;


        Speed = 1.8f;
        CurrentSpeed = 1.8f;

        CurrentStates = new List<CharacterStateType>();
        CurrentAbility = new List<CharacterAbilityType>();
        IgnoreStateList = new List<CharacterStateType>();
        BaseState = CharacterStateType.E_Idle;
    }

    public void InitStatus(MonsterTable table)
    {
        Damage = table._Damage;
        CurrentDamage = Damage;

        Armor = table._Armor;
        CurrentArmor = Armor;

        Hp = table._Hp;
        CurrentHp = Hp;

        Speed = table._Speed;
        CurrentSpeed = Speed;
    }

    public void PrepareState()
    {
        CurrentStates.Clear();
        CurrentSpeed = Speed;
        CurrentDamage = Damage;
    }

    public void PrepareAbility()
    {
        CurrentAbility.Clear();
    }
}
