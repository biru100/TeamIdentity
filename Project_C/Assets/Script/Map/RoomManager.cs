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
        if(Way.Contains(way))
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
        SamplingMapList("WoodMap");
        CreateMap();
    }

    public Room CurrentRoom { get; set; }
    public List<RoomContainer> AllRoom { get; protected set; }

    Dictionary<List<MapWay>, TileMapData> mapList;

    List<RoomContainer> roomFactoryProccess;
    List<RoomContainer> madeRooms;
    int maximumRoom = 50;

    void SamplingMapList(string mapThema)
    {
        mapList = new Dictionary<List<MapWay>, TileMapData>();
        TextAsset[] mapTxtDatas = Resources.LoadAll<TextAsset>("Map");

        foreach(var txt in mapTxtDatas)
        {
            TileMapData mapData = JsonUtility.FromJson<TileMapData>(txt.text);
            if(mapData.mapTheme == mapThema)
            {
                List<MapWay> ways = new List<MapWay>();

                foreach(var way in WayList)
                {
                    if((mapData.mapWay & (int)way) != 0)
                    {
                        ways.Add(way);
                    }
                }

                mapList.Add(ways, mapData);
            }
        }
    }

    void TransitionRoomFactoryProccess(RoomContainer current)
    {
        int randomConnectrionCount = Mathf.Min(Random.Range(0, 5 - current.Way.Count), maximumRoom 
            - roomFactoryProccess.Count
            - madeRooms.Count);

        List<MapWay> otherList = WayList.Except(current.Way).ToList();

        for (int i = 0; i < randomConnectrionCount; ++i)
        {
            int index = Random.Range(0, otherList.Count);
            MapWay currentWay = otherList[index];

            if(ConnectNearRoom(current, currentWay))
                current.Way.Add(otherList[index]);

            otherList.RemoveAt(index);
        }

        madeRooms.Add(current);
    }

    bool ConnectNearRoom(RoomContainer current, MapWay currentWay)
    {
        Vector2Int wayDirIndex = current.RoomIndex + WayDirectionSet[currentWay];

        RoomContainer container = madeRooms.Find((r) => r.RoomIndex == wayDirIndex);

        if (container == null)
        {
            container = roomFactoryProccess.Find((r) => r.RoomIndex == wayDirIndex);

            if (container == null)
            {
                roomFactoryProccess.Add(new RoomContainer(wayDirIndex, new List<MapWay>() { WayInverseSet[currentWay] }));
            }
            else
            {
                container.Way.Add(WayInverseSet[currentWay]);
            }

            return true;
        }
        else if(container.Way.Contains(WayInverseSet[currentWay]))
        {
            return true;
        }

        return false;
    }

    void ChooseMapInstance(RoomContainer current)
    {
        List<List<MapWay>> keys = mapList.Keys.Where((k) => current.Way.Count == k.Count 
            && k.Intersect(current.Way).Count() == current.Way.Count).ToList();

        current.RoomInstance = Room.CreateRoom(mapList[keys[Random.Range(0, keys.Count)]].mapName, current.RoomIndex);
    }

    void CreateMap()
    {
        roomFactoryProccess = new List<RoomContainer>();
        madeRooms = new List<RoomContainer>();

        RoomContainer first = new RoomContainer(Vector2Int.zero, new List<MapWay>() { MapWay.E_TOP });
        //RoomContainer boss = new RoomContainer(new Vector2Int((Random.Range(0, 2) - 1) * Random.Range(rangeX.x, rangeX.y),
        //    (Random.Range(0, 2) - 1) * Random.Range(rangeY.x, rangeY.y)), (int)MapWay.E_BOTTOM);


        madeRooms.Add(first);

        Vector2Int firstIndex;
        first.GetWayDestinationIndex(MapWay.E_TOP, out firstIndex);

        roomFactoryProccess.Add(new RoomContainer(firstIndex, new List<MapWay>() { WayInverseSet[MapWay.E_TOP]}));

        RoomContainer current;

        while (roomFactoryProccess.Count != 0)
        {
            current = roomFactoryProccess[0];
            roomFactoryProccess.RemoveAt(0);

            TransitionRoomFactoryProccess(current);
        }

        foreach(var roomContainer in madeRooms)
        {
            ChooseMapInstance(roomContainer);
        }

        AllRoom = madeRooms;

        CurrentRoom = AllRoom[0].RoomInstance;
        CurrentRoom.SetActiveMap(true);
    }

    public void CreatePlayer()
    {
        Player player = Instantiate(ResourceManager.GetResource<GameObject>("Tiles/Player")).GetComponent<Player>();
        player.transform.position = CurrentRoom.transform.position;
    }

    public void ChangeRoom(MapWay way)
    {
        RoomContainer rc = AllRoom.Find((r) => r.RoomIndex == CurrentRoom.RoomIndex + WayDirectionSet[way]);

        if (rc != null)
        {
            CurrentRoom.SetActiveMap(false);
            CurrentRoom = rc.RoomInstance;
            CurrentRoom.SetActiveMap(true);
        }
    }
}
