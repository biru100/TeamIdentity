using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshObstacle))]
[RequireComponent(typeof(BoxCollider))]
public class Door : MonoBehaviour
{
    public MapWay DoorType;
    public Room OwnerRoom { get; set; }
    public NavMeshObstacle Obstacle { get; protected set; }
    public BoxCollider Collider { get; protected set; }

    public Vector3 GetIntoPosition {
        get
        {
            Vector2Int doorNormal = RoomManager.WayDirectionSet[RoomManager.WayInverseSet[DoorType]];
            return transform.position + new Vector3(doorNormal.x * Isometric.IsometricGridSize, 0f, doorNormal.y * Isometric.IsometricGridSize) * 2f;
        }
    }

    private void Awake()
    {
        Obstacle = GetComponent<NavMeshObstacle>();
        Collider = GetComponent<BoxCollider>();
    }

    public void Update()
    {
        if (OwnerRoom == null)
            return;

        if(OwnerRoom.RoomAllEntitys.Count == 0)
        {
            Obstacle.enabled = false;
            Collider.enabled = true;
        }
    }

    public void OnTriggerEnter(Collider collider)
    {

        if (Player.CurrentPlayer == null)
            return;

        if(Player.CurrentPlayer.gameObject == collider.gameObject)
        {
            Debug.Log("change room");
            RoomManager.ChangeRoom(DoorType);

            Door intoDoor = RoomManager.Instance.CurrentRoom.RoomDoors.Find((d) => d.DoorType == RoomManager.WayInverseSet[DoorType]);

            if (intoDoor != null)
            {
                Vector3 newPos = intoDoor.GetIntoPosition;
                Player.CurrentPlayer.transform.position = newPos;
                Player.CurrentPlayer.NavAgent.Warp(newPos);
            }
            else
            {
                Debug.Log("맵 이상하다");
            }
        }
    }
}
