using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public interface IRoomFactory
{
    List<RoomContainer> CreateMap(ThemeTable data);
}

public class TestRoomFactory : IRoomFactory
{

    public string MapName { get; set; }

    public TestRoomFactory(string mapName)
    {
        MapName = mapName;
    }


    public List<RoomContainer> CreateMap(ThemeTable data)
    {
        RoomContainer testMap = new RoomContainer(Vector2Int.zero, new List<MapWay>());
        testMap.RoomInstance = Room.CreateRoom(MapName, Vector2Int.zero);
        return new List<RoomContainer>() { testMap };
    }
}

public class RogueRoomFactory : IRoomFactory
{
    ThemeTable currentTheme;

    Dictionary<List<MapWay>, TileMapData> mapList;

    List<RoomContainer> roomFactoryProccess;
    List<RoomContainer> madeRooms;

    Vector2Int mapIndexMin = Vector2Int.zero;
    Vector2Int mapIndexMax = Vector2Int.zero;

    List<MapWay> ConvertWayList(string mapName)
    {
        TileMapData mapData = JsonUtility.FromJson<TileMapData>(ResourceManager.GetResource<TextAsset>("Map/" + mapName).text);

        List<MapWay> ways = new List<MapWay>();

        foreach (var way in RoomManager.WayList)
        {
            if ((mapData.mapWay & (int)way) != 0)
            {
                ways.Add(way);
            }
        }

        return ways;
    }

    void SamplingMapList(string mapThema)
    {
        mapList = new Dictionary<List<MapWay>, TileMapData>();
        TextAsset[] mapTxtDatas = Resources.LoadAll<TextAsset>("Map");

        foreach (var txt in mapTxtDatas)
        {
            TileMapData mapData = JsonUtility.FromJson<TileMapData>(txt.text);
            if (mapData.mapTheme == mapThema)
            {
                List<MapWay> ways = new List<MapWay>();

                foreach (var way in RoomManager.WayList)
                {
                    if ((mapData.mapWay & (int)way) != 0)
                    {
                        ways.Add(way);
                    }
                }

                mapList.Add(ways, mapData);
            }
        }
    }

    int CalculateRoomCountByWeight(int currentRoomWay)
    {
        int maxWeight = 0;

        for (int i = currentRoomWay - 1; i < currentTheme._WayForWeight.Length; ++i)
        {
            maxWeight += currentTheme._WayForWeight[i];
        }

        int randomWeight = Random.Range(0, maxWeight);

        int randomRoomCount = 0;

        for (int i = currentRoomWay - 1; i < currentTheme._WayForWeight.Length; ++i)
        {
            if (randomWeight < currentTheme._WayForWeight[i])
            {
                randomRoomCount = i + 1;
                break;
            }

            randomWeight -= currentTheme._WayForWeight[i];
        }

        return randomRoomCount - currentRoomWay;
    }

    int CalculateTransitionRoomWayCount(int currentRoomWay)
    {
        int remainMinRoomCount = currentTheme._MinRoomCount
            - roomFactoryProccess.Count
            - madeRooms.Count;

        int remainMaxRoomCount = currentTheme._MaxRoomCount
            - roomFactoryProccess.Count
            - madeRooms.Count;

        if (remainMinRoomCount > 0)
        { 
            return Mathf.Clamp(CalculateRoomCountByWeight(currentRoomWay), 1, remainMaxRoomCount);
        }
        else if(remainMaxRoomCount > 0)
        {
            return Mathf.Clamp(CalculateRoomCountByWeight(currentRoomWay), 0, remainMaxRoomCount);
        }
        else
        {
            return 0;
        }
    }

    void TransitionRoomFactoryProccess(RoomContainer current)
    {
        int randomConnectionCount = CalculateTransitionRoomWayCount(current.Way.Count);

        List<MapWay> otherList = RoomManager.WayList.Except(current.Way).Where((way) => roomFactoryProccess.FindIndex((r) => r.RoomIndex == current.RoomIndex + RoomManager.WayDirectionSet[way]) == -1 &&
        madeRooms.FindIndex((r) => r.RoomIndex == current.RoomIndex + RoomManager.WayDirectionSet[way]) == -1).ToList();

        for (int i = 0; i < randomConnectionCount; ++i)
        {
            if (otherList.Count == 0)
                break;

            int index = Random.Range(0, otherList.Count);
            MapWay currentWay = otherList[index];

            if (ConnectNearRoom(current, currentWay))
                current.Way.Add(otherList[index]);

            otherList.RemoveAt(index);
        }

        madeRooms.Add(current);
    }

    bool ConnectNearRoom(RoomContainer current, MapWay currentWay)
    {
        Vector2Int wayDirIndex = current.RoomIndex + RoomManager.WayDirectionSet[currentWay];

        RoomContainer container = madeRooms.Find((r) => r.RoomIndex == wayDirIndex);

        if (container == null)
        {
            container = roomFactoryProccess.Find((r) => r.RoomIndex == wayDirIndex);

            if (container == null)
            {
                roomFactoryProccess.Add(new RoomContainer(wayDirIndex, new List<MapWay>() { RoomManager.WayInverseSet[currentWay] }));

                mapIndexMin.x = Mathf.Min(mapIndexMin.x, wayDirIndex.x);
                mapIndexMin.y = Mathf.Min(mapIndexMin.y, wayDirIndex.y);

                mapIndexMax.x = Mathf.Max(mapIndexMax.x, wayDirIndex.x);
                mapIndexMax.y = Mathf.Max(mapIndexMax.y, wayDirIndex.y);
            }
            else
            {
                container.Way.Add(RoomManager.WayInverseSet[currentWay]);
            }

            return true;
        }
        else if (container.Way.Contains(RoomManager.WayInverseSet[currentWay]))
        {
            return true;
        }

        return false;
    }

    void ChooseMapInstance(RoomContainer current)
    {
        if (current.RoomInstance != null)
            return;

        List<List<MapWay>> keys = mapList.Keys.Where((k) => current.Way.Count == k.Count
            && k.Intersect(current.Way).Count() == current.Way.Count).ToList();

        current.RoomInstance = Room.CreateRoom(mapList[keys[Random.Range(0, keys.Count)]].mapName, current.RoomIndex);
    }

    void SetMapInstance(RoomContainer current, string mapName)
    {
        if (current.RoomInstance != null)
            return;
        current.RoomInstance = Room.CreateRoom(mapName, current.RoomIndex);
    }

    RoomContainer DeterminantSpecificRoom(List<RoomContainer> ignoreParent, List<MapWay> roomWay, string roomName)
    {
        Vector2Int specificMapAdditionalDirection = RoomManager.WayDirectionSet[RoomManager.WayInverseSet[roomWay[0]]];
        Vector2Int specificMapIncludeAxisOutLine = specificMapAdditionalDirection;
        specificMapIncludeAxisOutLine.x *= specificMapIncludeAxisOutLine.x > 0 ? mapIndexMax.x  : -(mapIndexMin.x );
        specificMapIncludeAxisOutLine.y *= specificMapIncludeAxisOutLine.y > 0 ? mapIndexMax.y  : -(mapIndexMin.y );


        List<RoomContainer> outLineRooms = madeRooms.Where((r) => (specificMapIncludeAxisOutLine.x != 0 && specificMapIncludeAxisOutLine.x == r.RoomIndex.x) ||
        (specificMapIncludeAxisOutLine.y != 0 && specificMapIncludeAxisOutLine.y == r.RoomIndex.y)).
        Except(ignoreParent).ToList();

        RoomContainer specificParentRoom = outLineRooms[Random.Range(0, outLineRooms.Count)];
        MapWay ToBossWay = RoomManager.WayDirectionSet.First((p) => p.Value == specificMapAdditionalDirection).Key;

        specificParentRoom.Way.Add(ToBossWay);

        RoomContainer specificRoom = new RoomContainer(specificParentRoom.RoomIndex + specificMapAdditionalDirection, roomWay);
        madeRooms.Add(specificRoom);

        SetMapInstance(specificRoom, roomName);

        return specificRoom;
    }

    public List<RoomContainer> CreateMap(ThemeTable data)
    {
        currentTheme = data;

        SamplingMapList(data._Name);

        roomFactoryProccess = new List<RoomContainer>();
        madeRooms = new List<RoomContainer>();

        RoomContainer first = new RoomContainer(Vector2Int.zero, new List<MapWay>() { MapWay.E_TOP });

        SetMapInstance(first, data._StartRoomName);

        madeRooms.Add(first);

        Vector2Int firstIndex;
        first.GetWayDestinationIndex(MapWay.E_TOP, out firstIndex);

        roomFactoryProccess.Add(new RoomContainer(firstIndex, new List<MapWay>() { RoomManager.WayInverseSet[MapWay.E_TOP] }));

        RoomContainer current;

        while (roomFactoryProccess.Count != 0)
        {
            current = roomFactoryProccess[0];
            roomFactoryProccess.RemoveAt(0);

            TransitionRoomFactoryProccess(current);
        }

        List<RoomContainer> specificRoomList = new List<RoomContainer>()
        {
            first
        };

        specificRoomList.Add(DeterminantSpecificRoom(specificRoomList, ConvertWayList(currentTheme._BossRoomName), currentTheme._BossRoomName));

        foreach (var roomContainer in madeRooms)
        {
            ChooseMapInstance(roomContainer);
        }

        return madeRooms;
    }
}