//이 코드는 엑셀 파서에 의해 자동 생성됨. 
using System;
using System.IO;

public class MonsterTable { 
   public int _Index;
   public string _Name;
   public string _krName;
   public float _Damage;
   public float _Hp;
   public float _Speed;
   public float _Armor;
   public string _Lore;
   public CharacterAbilityType[] _Abilities = new CharacterAbilityType[6];
   public float[] _Parameter = new float[2];
   public static MonsterTable Load(string[] parts) {
       int i = 0;
       MonsterTable p = new MonsterTable();
       p._Index = int.Parse(parts[i++]);
       p._Name = parts[i++];
       p._krName = parts[i++];
       p._Damage = float.Parse(parts[i++]);
       p._Hp = float.Parse(parts[i++]);
       p._Speed = float.Parse(parts[i++]);
       p._Armor = float.Parse(parts[i++]);
       p._Lore = parts[i++];
       p._Abilities[0] = (CharacterAbilityType)System.Enum.Parse(typeof(CharacterAbilityType),parts[i++]);
       p._Abilities[1] = (CharacterAbilityType)System.Enum.Parse(typeof(CharacterAbilityType),parts[i++]);
       p._Abilities[2] = (CharacterAbilityType)System.Enum.Parse(typeof(CharacterAbilityType),parts[i++]);
       p._Abilities[3] = (CharacterAbilityType)System.Enum.Parse(typeof(CharacterAbilityType),parts[i++]);
       p._Abilities[4] = (CharacterAbilityType)System.Enum.Parse(typeof(CharacterAbilityType),parts[i++]);
       p._Abilities[5] = (CharacterAbilityType)System.Enum.Parse(typeof(CharacterAbilityType),parts[i++]);
       p._Parameter[0] = float.Parse(parts[i++]);
       p._Parameter[1] = float.Parse(parts[i++]);

    return p;
    }
}
