using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapInterface : UIBase<MinimapInterface>
{
    public Sprite currentRoomS;
    public Sprite roomS;
    public Sprite vS;
    public Sprite hS;

    public Image CurrentRoomImg;

    // Start is called before the first frame update
    void Start()
    {
        List<RoomContainer> rooms = RoomManager.Instance.AllRoom;

        foreach (var room in rooms)
        {
            GameObject go = new GameObject(room.RoomIndex.ToString());
            go.transform.parent = transform;
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = new Vector3(room.RoomIndex.x * 54f, room.RoomIndex.y * 54f, 0f);
            go.AddComponent<Image>().sprite = roomS;
            (go.transform as RectTransform).sizeDelta = Vector2.one * 36f;


            for (int i = 0; i < 4; ++i)
            {
                MapWay way= (MapWay)(1 << i);
                if(room.Way.Contains(way))
                {
                    Vector2Int dir = RoomManager.WayDirectionSet[way];

                    GameObject bridge = new GameObject(room.RoomIndex.ToString() + "_" + (room.RoomIndex + RoomManager.WayDirectionSet[way]).ToString());
                    bridge.transform.localScale = Vector3.one;
                    bridge.transform.parent = transform;
                    bridge.transform.localPosition = go.transform.localPosition + new Vector3(dir.x * 27f, dir.y * 27f, 0f);

                    bridge.AddComponent<Image>().sprite = i % 2 == 1 ? hS : vS;
                    (bridge.transform as RectTransform).sizeDelta = i % 2 == 1 ? new Vector2(8f, 18f) : new Vector2(18f, 8f);
                }
            }
        }

        GameObject cr = new GameObject(RoomManager.Instance.CurrentRoom.RoomIndex.ToString());
        cr.transform.parent = transform;
        cr.transform.localScale = Vector3.one;
        cr.transform.localPosition = new Vector3(RoomManager.Instance.CurrentRoom.RoomIndex.x * 54f, RoomManager.Instance.CurrentRoom.RoomIndex.y * 54f, 0f);
        CurrentRoomImg = cr.AddComponent<Image>();
        CurrentRoomImg.sprite = currentRoomS;
        (cr.transform as RectTransform).sizeDelta = Vector2.one * 36f;
    }

    private void Update()
    {
        CurrentRoomImg.transform.localPosition = new Vector3(RoomManager.Instance.CurrentRoom.RoomIndex.x * 54f, RoomManager.Instance.CurrentRoom.RoomIndex.y * 54f, 0f);
    }


}
