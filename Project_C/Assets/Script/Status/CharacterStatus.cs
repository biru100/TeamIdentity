using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    public void UpdateStatus(CharacterNotifyEvent notify)
    {
        if(notify.Type == CharacterNotifyType.E_Damage && CurrentHp > 0f)
        {
            CurrentHp = Mathf.Max(CurrentHp - (float)notify.Data, 0f);
            if(CurrentHp <= 0f)
            {
                Owner.ConsumeNotifyEvent(notify);
                Owner.AddNotifyEvent(new CharacterNotifyEvent(CharacterNotifyType.E_Dead, null));
            }
        }
    }
}
