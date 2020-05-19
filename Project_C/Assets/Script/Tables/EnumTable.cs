//이 코드는 엑셀 파서에 의해 자동 생성됨. 
using System;


 public enum CharacterStateType
 {
    E_Idle = 0 , //  
    E_IncreaseDamage = 101 , //  
    E_DecreaseDamage = 102 , //  
    E_IncreaseSpeed = 103 , //  
    E_Slow = 104 , //  
    E_Stun = 105 , //  
    E_Hold = 106 , //  
    E_Silence = 107 , //  
    E_Invincibility = 108 , //  
    E_TauntInvincibility = 109 , //  
    E_SuperArmor = 110 , //  
    E_Hit = 111 , //  
    E_Dead = 112 , //  
  }
 public enum CharacterAbilityType
 {
    E_None = 0 , //  
    E_IncreaseDamage = 201 , //  
    E_DecreaseDamage = 202 , //  
    E_IncreaseSpeed = 203 , //  
    E_DecreaseSpeed = 204 , //  
    E_GiveStun = 205 , //  
    E_GiveHold = 206 , //  
    E_GiveSilence = 207 , //  
    E_GiveInvincibility = 208 , //  
    E_Taunt = 209 , //  
    E_GiveHeal = 210 , //  
    E_Spawn = 211 , //  
    E_NonAttack = 212 , //  
    E_NonMove = 213 , //  
    E_GiveDraw = 214 , //  
    E_CardBreak = 215 , //  
  }
 public enum CardTargetType
 {
    E_NonTarget = 0 , //  
    E_Target = 1 , //  
    E_Point = 2 , //  
  }
 public enum CardRangeType
 {
    E_None = 0 , //  
    E_PlayerRelativeCircularSector = 1 , //  
    E_PlayerRelativeCircle = 2 , //  
    E_PointCircularSector = 3 , //  
    E_PointCircle = 4 , //  
  }
