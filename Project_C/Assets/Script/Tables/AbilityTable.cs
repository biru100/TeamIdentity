//이 코드는 엑셀 파서에 의해 자동 생성됨. 
using System;
using System.IO;

public class AbilityTable { 
   public int _Index;
   public string _Name;
   public string _StatePath;
   public static AbilityTable Load(string[] parts) {
       int i = 0;
       AbilityTable p = new AbilityTable();
       p._Index = int.Parse(parts[i++]);
       p._Name = parts[i++];
       p._StatePath = parts[i++];

    return p;
    }
}
