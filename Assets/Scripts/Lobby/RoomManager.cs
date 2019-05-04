using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RoomManager : Photon.PunBehaviour {

    #region Singleton
    public static RoomManager instance;
    private void Awake()
    {
        if (instance != null)
            Debug.Log("Multiple RoomManager. Something went wrong");
        instance = this;
    }
    #endregion

    [SerializeField]
    private Text roomNameText;
    [SerializeField]
    private Button leaveButton;
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private GameObject roomPlayerSlotPrefab;
    [SerializeField]
    private Transform playerLayoutGroup;

    private List<RoomPlayer> roomPlayers;


    // called when you join the room and the RoomInterface is activated.
    private void OnEnable()
    {
        if (!PhotonNetwork.inRoom)
            return;
        roomPlayers = new List<RoomPlayer>();
        roomNameText.text = PhotonNetwork.room.Name;
        if (PhotonNetwork.isMasterClient)
        {
            startButton.gameObject.SetActive(true);
        }

        CreateLocalRoomPlayer();
    }

    // called when you left the room and the RoomInterface is deactivated.
    private void OnDisable()
    {
        RoomPlayerSlot[] slots = playerLayoutGroup.GetComponentsInChildren<RoomPlayerSlot>();
        for (int i = 0;  i < slots.Length; i++)
        {
            Destroy(slots[i].gameObject);
        }
        roomPlayers = null;
    }

    // called when other players join the room
    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        if (newPlayer.NickName == null)
            Debug.Log("A new player entered the room");
        else
            Debug.Log(newPlayer.NickName + " entered the room");
    }

    // called when other players leave the room
    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        Debug.Log(otherPlayer.CustomProperties["PlayerName"] + " left the room");
        RemovePlayerSlot(otherPlayer);
    }

    public void CreateLocalRoomPlayer()
    {
        PhotonNetwork.Instantiate("RoomPlayer", Vector3.zero, Quaternion.identity, 0);
    }

    public void CreatePlayerSlot(RoomPlayer newRoomPlayer, bool isLocal)
    {
        GameObject roomPlayerSlotObj = Instantiate(roomPlayerSlotPrefab, playerLayoutGroup, false);
        RoomPlayerSlot roomPlayerSlot = roomPlayerSlotObj.GetComponent<RoomPlayerSlot>();
        newRoomPlayer.AssignRoomPlayerSlot(roomPlayerSlot);
        roomPlayerSlot.Initialize(newRoomPlayer);

        if (isLocal)
        {
            roomPlayerSlot.SetAsLocal();
        }
        roomPlayers.Add(newRoomPlayer);
    }

    
    public void RemovePlayerSlot(PhotonPlayer otherPlayer)
    {
        for(int i = 0; i < roomPlayers.Count; i++)
        {
            if (otherPlayer == roomPlayers[i].PhotonPlayer)
            {
                Destroy(roomPlayers[i].RoomPlayerSlot.gameObject);
            }
        }
    }
    

    public void OnStartButtonClicked()
    {
        bool readyToStart = true;
        foreach (PhotonPlayer photonPlayer in PhotonNetwork.playerList)
        {
            if (!(bool)photonPlayer.CustomProperties["IsReady"])
            {
                Debug.Log("Player " + photonPlayer.NickName + " not ready.");
                readyToStart = false;
            }
        }
        if (readyToStart)
        {
            PhotonNetwork.LoadLevel("MainGameOnline");
            PhotonNetwork.room.IsOpen = false;
            PhotonNetwork.room.IsVisible = false;
        }
    }


    public void OnLeaveButtonClicked()
    {
        LobbyManager.instance.LeaveRoom();
    }

}
