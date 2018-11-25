using UnityEngine;
using UnityEngine.UI;

public class RoomSnapshot : MonoBehaviour {

    [SerializeField]
    private Text textRoomName;
    [SerializeField]
    private Text textRoomHost;
    [SerializeField]
    private Text textMatchSize;

    RoomInfo roomInfo;


    public void Initialize(RoomInfo room)
    {
        textRoomName.text = room.Name;
        textRoomHost.text = "";
        textMatchSize.text = room.PlayerCount.ToString() + "/" + room.MaxPlayers.ToString();
        roomInfo = room;
    }

    public void OnRoomSelected()
    {
        LobbyPanel.instance.NewRoomSelected(roomInfo);
    }
}
