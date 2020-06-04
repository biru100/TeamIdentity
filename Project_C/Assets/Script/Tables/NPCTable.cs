//이 코드는 엑셀 파서에 의해 자동 생성됨. 
using System;
using System.IO;

public class NPCTable { 
   public int _Index;
   public string _Name;
   public string _krName;
   public bool _PersistenceAbility;
   public float[] _Parameter = new float[4];
   public static NPCTable Load(string[] parts) {
       int i = 0;
       NPCTable p = new NPCTable();
       p._Index = int.Parse(parts[i++]);
       p._Name = parts[i++];
       p._krName = parts[i++];
       p._PersistenceAbility = bool.Parse(parts[i++]);
       p._Parameter[0] = float.Parse(parts[i++]);
       p._Parameter[1] = float.Parse(parts[i++]);
       p._Parameter[2] = float.Parse(parts[i++]);
       p._Parameter[3] = float.Parse(parts[i++]);

    return p;
    }
}
