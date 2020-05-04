using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Room : MonoBehaviour
{
    public Vector2Int RoomIndex { get; protected set; }
    public List<Character> RoomAllEntitys { get; protected set; }
    public List<Door> RoomDoors { get; protected set; }
    public List<MapWay> RoomWays { get; protected set; }
    public NavMeshData NavData { get; protected set; }
    public TileMapData MapData { get; protected set; }

    public static Room CreateRoom(string roomName, Vector2Int index)
    {

        string jsonData = ResourceManager.GetResource<TextAsset>("Map/" + roomName).text;
        TileMapData data = JsonUtility.FromJson<TileMapData>(jsonData);

        Room room = new GameObject(roomName).AddComponent<Room>();
        room.RoomIndex = index;
        room.RoomAllEntitys = new List<Character>();
        room.RoomDoors = new List<Door>();
        room.RoomWays = new List<MapWay>();

        room.MapData = data;

        foreach (var pair in data.mapData.data)
        {
            room.LoadMapElement(EffectiveUtility.VectorMultiple(new Vector3(pair.index.x, pair.index.y, pair.index.z), Isometric.IsometricTileSize),
                ResourceManager.GetResource<GameObject>("Tiles/" + pair.tag));
        }

        room.NavData = DynamicNavigation.Instance.BuildNavigation(room.transform);
        room.gameObject.SetActive(false);
        return room;
    }

    private void Update()
    { 
        for(int i = 0; i < RoomAllEntitys.Count;)
        {
            if (RoomAllEntitys[i] == null)
            {
                RoomAllEntitys.RemoveAt(i);
            }
            else
                ++i;
        }
    }

    public void SetActiveMap(bool isActive)
    {
        if(isActive == true)
        {
            DynamicNavigation.Instance.SetNavMeshData(NavData);
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void LoadMapElement(Vector3 isoPos, GameObject go)
    {
        Vector3Int index = EffectiveUtility.IsoPositionToIndex(isoPos);

        if(go.GetComponent<Player>() != null)
        {
            return;
        }

        GameObject instance = Instantiate(go, isoPos, Quaternion.identity, transform);

        Character entity = instance.GetComponent<Character>();
        if (entity != null)
        {
            RoomAllEntitys.Add(entity);
            entity.OwnerRoom = this;
        }

        Door door = instance.GetComponent<Door>();
        if (door != null)
        {
            RoomDoors.Add(door);
            RoomWays.Add(door.DoorType);
            door.OwnerRoom = this;
        }
    }
}
