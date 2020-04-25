//이 코드는 엑셀 파서에 의해 자동 생성됨. 
using System;
using System.IO;

public class CardTable { 
   public int _Index;
   public string _Cardname;
   public string _Cardtext;
   public string _CardImagePath;
   public int _Cardcost;
   public string _FSM;
   public float[] _Parameter = new float[2];
   public static CardTable Load(string[] parts) {
       int i = 0;
       CardTable p = new CardTable();
       p._Index = int.Parse(parts[i++]);
       p._Cardname = parts[i++];
       p._Cardtext = parts[i++];
       p._CardImagePath = parts[i++];
       p._Cardcost = int.Parse(parts[i++]);
       p._FSM = parts[i++];
       p._Parameter[0] = float.Parse(parts[i++]);
       p._Parameter[1] = float.Parse(parts[i++]);

    return p;
    }
}
