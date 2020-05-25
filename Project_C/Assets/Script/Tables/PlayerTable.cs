//이 코드는 엑셀 파서에 의해 자동 생성됨. 
using System;
using System.IO;

public class PlayerTable { 
   public int _Index;
   public string _Name;
   public float _Damage;
   public float _Hp;
   public float _Speed;
   public float _Armor;
   public int _HandCount;
   public int _DeckCount;
   public static PlayerTable Load(string[] parts) {
       int i = 0;
       PlayerTable p = new PlayerTable();
       p._Index = int.Parse(parts[i++]);
       p._Name = parts[i++];
       p._Damage = float.Parse(parts[i++]);
       p._Hp = float.Parse(parts[i++]);
       p._Speed = float.Parse(parts[i++]);
       p._Armor = float.Parse(parts[i++]);
       p._HandCount = int.Parse(parts[i++]);
       p._DeckCount = int.Parse(parts[i++]);

    return p;
    }
}
