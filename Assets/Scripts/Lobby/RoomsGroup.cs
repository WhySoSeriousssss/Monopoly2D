using System.Collections.Generic;
using UnityEngine;

public class RoomsGroup : MonoBehaviour {

    [SerializeField]
    private GameObject RoomSnapshotPrefab;

    public void UpdateRoomInfo(RoomInfo[] roomList)
    {
        // remove the old room list
        RoomSnapshot[] rooms = GetComponentsInChildren<RoomSnapshot>();
        foreach(RoomSnapshot room in rooms)
        {
            Destroy(room.gameObject);
        }

        foreach(RoomInfo room in roomList)
        {
            GameObject roomObject = Instantiate(RoomSnapshotPrefab, transform, false);
            roomObject.GetComponent<RoomSnapshot>().Initialize(room);
        }
    }
}
