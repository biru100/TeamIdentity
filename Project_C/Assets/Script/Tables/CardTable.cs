//이 코드는 엑셀 파서에 의해 자동 생성됨. 
using System;
using System.IO;

public class CardTable { 
   public int _Index;
   public string _Name;
   public string _krName;
   public CardTargetType _TargetType;
   public string _Lore;
   public string _ImagePath;
   public int _Cost;
   public string _FSM;
   public int _ParameterCount;
   public float[] _Parameter = new float[2];
   public bool[] _IsVariable = new bool[2];
   public string _CardRangeSprite;
   public CardRangeType _RangeType;
   public static CardTable Load(string[] parts) {
       int i = 0;
       CardTable p = new CardTable();
       p._Index = int.Parse(parts[i++]);
       p._Name = parts[i++];
       p._krName = parts[i++];
       p._TargetType = (CardTargetType)System.Enum.Parse(typeof(CardTargetType),parts[i++]);
       p._Lore = parts[i++];
       p._ImagePath = parts[i++];
       p._Cost = int.Parse(parts[i++]);
       p._FSM = parts[i++];
       p._ParameterCount = int.Parse(parts[i++]);
       p._Parameter[0] = float.Parse(parts[i++]);
       p._Parameter[1] = float.Parse(parts[i++]);
       p._IsVariable[0] = bool.Parse(parts[i++]);
       p._IsVariable[1] = bool.Parse(parts[i++]);
       p._CardRangeSprite = parts[i++];
       p._RangeType = (CardRangeType)System.Enum.Parse(typeof(CardRangeType),parts[i++]);

    return p;
    }
}
