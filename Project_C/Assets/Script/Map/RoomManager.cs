using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomContainer
{
    public Vector2Int RoomIndex { get; set; }
    public List<MapWay> Way { get; set; }

    public Room RoomInstance { get; set; }

    public RoomContainer(Vector2Int roomIndex, List<MapWay> way)
    {
        RoomIndex = roomIndex;
        Way = way;
    }

    public bool GetWayDestinationIndex(MapWay way, out Vector2Int index)
    {
        if (Way.Contains(way))
        {
            index = RoomIndex + RoomManager.WayDirectionSet[way];
            return true;
        }
        index = Vector2Int.zero;
        return false;
    }
}

public class RoomManager : BehaviorSingleton<RoomManager>
{
    public static readonly Dictionary<MapWay, Vector2Int> WayDirectionSet = new Dictionary<MapWay, Vector2Int>()
    {
        { MapWay.E_LEFT, new Vector2Int(-1, 0)},
        { MapWay.E_TOP, new Vector2Int(0, 1)},
        { MapWay.E_RIGHT, new Vector2Int(1, 0)},
        { MapWay.E_BOTTOM, new Vector2Int(0, -1)}
    };

    public static readonly List<MapWay> WayList = new List<MapWay>()
    {
        MapWay.E_LEFT,
        MapWay.E_TOP,
        MapWay.E_RIGHT,
        MapWay.E_BOTTOM
    };

    public static readonly Dictionary<MapWay, MapWay> WayInverseSet = new Dictionary<MapWay, MapWay>()
    {
        { MapWay.E_LEFT, MapWay.E_RIGHT},
        { MapWay.E_TOP, MapWay.E_BOTTOM},
        { MapWay.E_RIGHT, MapWay.E_LEFT},
        { MapWay.E_BOTTOM, MapWay.E_TOP}
    };



    protected override void Init()
    {
        base.Init();
    }

    public Room CurrentRoom { get; set; }
    public List<RoomContainer> AllRoom { get; protected set; }

    public static void CreateRogueMap()
    {
        ThemeTable firstTheme = DataManager.GetFirstData<ThemeTable>();
        Instance.AllRoom = new RogueRoomFactory().CreateMap(firstTheme);

        FactoringRoomLogData.GetInstance().Init();
    }

    public static void CreateTestMap(string mapName)
    {
        Instance.AllRoom = new TestRoomFactory(mapName).CreateMap(null);
    }

    public static void CreatePlayer()
    {
        Player player = Instantiate(ResourceManager.GetResource<GameObject>("Tiles/Player")).GetComponent<Player>();
        Instance.ChangeRoom(Instance.AllRoom[0].RoomInstance);
        player.transform.position = Instance.CurrentRoom.transform.position;
    }

    void ChangeRoom(Room room)
    {
        CurrentRoom?.SetActiveMap(false);
        CurrentRoom = room;
        CurrentRoom.SetActiveMap(true);

        PlayerStatus.CurrentStatus.CurrentManaCost = room.SupportCostCount;
        InGameInterface.Instance.DrawCard(room.SupportDrawCount);
        room.SupportCostCount = 0;
        room.SupportDrawCount = 0;

        ChangeRoomLogData.GetInstance().Init(room);
    }


    public static void ChangeRoom(MapWay way)
    {
        RoomContainer rc = Instance.AllRoom.Find((r) => r.RoomIndex == Instance.CurrentRoom.RoomIndex + WayDirectionSet[way]);

        if (rc != null)
        {
            Instance.ChangeRoom(rc.RoomInstance);
        }
    }
}
