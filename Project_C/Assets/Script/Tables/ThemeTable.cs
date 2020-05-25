//이 코드는 엑셀 파서에 의해 자동 생성됨. 
using System;
using System.IO;

public class ThemeTable { 
   public int _Index;
   public string _Name;
   public string _StartRoomName;
   public string _BossRoomName;
   public int _MinRoomCount;
   public int _MaxRoomCount;
   public int _TotalWayWeight;
   public int _RewardRoomCount;
   public int[] _WayForWeight = new int[4];
   public string[] _RewardRoomNames = new string[1];
   public static ThemeTable Load(string[] parts) {
       int i = 0;
       ThemeTable p = new ThemeTable();
       p._Index = int.Parse(parts[i++]);
       p._Name = parts[i++];
       p._StartRoomName = parts[i++];
       p._BossRoomName = parts[i++];
       p._MinRoomCount = int.Parse(parts[i++]);
       p._MaxRoomCount = int.Parse(parts[i++]);
       p._TotalWayWeight = int.Parse(parts[i++]);
       p._RewardRoomCount = int.Parse(parts[i++]);
       p._WayForWeight[0] = int.Parse(parts[i++]);
       p._WayForWeight[1] = int.Parse(parts[i++]);
       p._WayForWeight[2] = int.Parse(parts[i++]);
       p._WayForWeight[3] = int.Parse(parts[i++]);
       p._RewardRoomNames[0] = parts[i++];

    return p;
    }
}
