//이 코드는 엑셀 파서에 의해 자동 생성됨. 
using System;
using System.IO;

public class Sheet1 { 
   public int _Index;
   public string _Name;
   public static Sheet1 Load(string[] parts) {
       int i = 0;
       Sheet1 p = new Sheet1();
       p._Index = int.Parse(parts[i++]);
       p._Name = parts[i++];

    return p;
    }
}
