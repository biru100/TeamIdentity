//이 코드는 엑셀 파서에 의해 자동 생성됨. 
using System;
using System.IO;

public class StateTable { 
   public int _Index;
   public string _Name;
   public string _IconStatePath;
   public static StateTable Load(string[] parts) {
       int i = 0;
       StateTable p = new StateTable();
       p._Index = int.Parse(parts[i++]);
       p._Name = parts[i++];
       p._IconStatePath = parts[i++];

    return p;
    }
}
