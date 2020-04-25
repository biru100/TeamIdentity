using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

//public enum CharacterAbilityType
//{
//    E_IncreaseDamage,
//    E_DecreaseDamage,
//    E_IncreaseSpeed,
//    E_DecreaseSpeed,
//    E_GiveStun,
//    E_GiveHold,
//    E_GiveSilence,
//    E_GiveInvincibility,
//    E_Taunt,
//    E_GiveHeal,
//    E_Spawn,
//    E_NonAttack,
//    E_NonMove,
//    E_GiveDraw,
//    E_CardBreak
//}

public class CharacterTauntAbility : CharacterAbility
{
    public Dictionary<Character, CharacterState> _buffCharacters;

    //방 한테 몬스터 정보 불러옴 - 도발을 제외한 몬스터에게 도발무적 상태를 제공 
    public CharacterTauntAbility(Character owner)
        : base(CharacterAbilityType.E_Taunt, owner)
    {
        _buffCharacters = new Dictionary<Character, CharacterState>();
    }

    public override bool UpdateAbility()
    {
        if(base.UpdateAbility())
        {
            GiveTauntInvincibility();
            return true;
        }
        else
        {
            RemoveTauntInvincibility();
            return false;
        }
    }

    void GiveTauntInvincibility()
    {
        List<Character> characters = UnityEngine.Object.FindObjectsOfType<Character>()?.ToList();
        if (characters == null || characters.Count == 0)
            return;

        foreach (var character in characters)
        {
            if (character is Player)
                continue;

            if (!_buffCharacters.ContainsKey(character))
            {
                CharacterState invState = new CharacterState(CharacterStateType.E_TauntInvincibility, character);
                character.AddState(invState.Init());
                _buffCharacters.Add(character, invState);
            }
        }
    }

    void RemoveTauntInvincibility()
    {
        foreach (var pair in _buffCharacters)
        {
            pair.Value.ForcedDeleteState();
        }

        _buffCharacters.Clear();
    }

}

public class CharacterAbility
{
    public static readonly Dictionary<CharacterAbilityType, Func<Character, CharacterAbility>> CharacterAbilityBuilderSet = new Dictionary<CharacterAbilityType, Func<Character, CharacterAbility>>()
    {
        { CharacterAbilityType.E_IncreaseDamage, (c)=>new CharacterAbility(CharacterAbilityType.E_IncreaseDamage, c)},
        { CharacterAbilityType.E_DecreaseDamage, (c)=>new CharacterAbility(CharacterAbilityType.E_DecreaseDamage, c)},
        { CharacterAbilityType.E_IncreaseSpeed, (c)=>new CharacterAbility(CharacterAbilityType.E_IncreaseSpeed, c)},
        { CharacterAbilityType.E_DecreaseSpeed, (c)=>new CharacterAbility(CharacterAbilityType.E_DecreaseSpeed, c)},
        { CharacterAbilityType.E_CardBreak, (c)=>new CharacterAbility(CharacterAbilityType.E_CardBreak, c)},
        { CharacterAbilityType.E_GiveDraw, (c)=>new CharacterAbility(CharacterAbilityType.E_GiveDraw, c)},
        { CharacterAbilityType.E_GiveHeal, (c)=>new CharacterAbility(CharacterAbilityType.E_GiveHeal, c)},
        { CharacterAbilityType.E_GiveHold, (c)=>new CharacterAbility(CharacterAbilityType.E_GiveHold, c)},
        { CharacterAbilityType.E_GiveInvincibility, (c)=>new CharacterAbility(CharacterAbilityType.E_GiveInvincibility, c)},
        { CharacterAbilityType.E_GiveSilence, (c)=>new CharacterAbility(CharacterAbilityType.E_GiveSilence, c)},
        { CharacterAbilityType.E_GiveStun, (c)=>new CharacterAbility(CharacterAbilityType.E_GiveStun, c)},
        { CharacterAbilityType.E_NonAttack, (c)=>new CharacterAbility(CharacterAbilityType.E_NonAttack, c)},
        { CharacterAbilityType.E_NonMove, (c)=>new CharacterAbility(CharacterAbilityType.E_NonMove, c)},
        { CharacterAbilityType.E_Spawn, (c)=>new CharacterAbility(CharacterAbilityType.E_Spawn, c)},
        { CharacterAbilityType.E_Taunt, (c)=>new CharacterTauntAbility(c)},
    };


    public CharacterAbilityType AbilityType { get; protected set; }
    public Character Owner { get; protected set; }
    public CharacterStatus Status { get; protected set; }

    public CharacterAbility(CharacterAbilityType type, Character owner)
    {
        Status = owner.Status;
        Owner = owner;
        AbilityType = type;
    }

    public virtual bool UpdateAbility()
    {
        if(!Status.CurrentStates.Contains(CharacterStateType.E_Silence))
        {
            Status.CurrentAbility.Add(AbilityType);
            return true;
        }

        return false;
    }
}

