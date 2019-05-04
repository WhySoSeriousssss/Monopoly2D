using ExitGames.Client.Photon;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : Photon.PunBehaviour{

    [SerializeField]
    private RoomsGroup roomsGroup;
    [SerializeField]
    private GameObject roomInterface;

    string _gameVersion = "1";

    private int _initialMoney;


    #region Singleton
    public static LobbyManager instance;
    private void Awake()
    {
        if (instance != null)
            Debug.Log("Multiple LobbyManager. Something went wrong");
        instance = this;
    }
    #endregion

    private void Start()
    {
        PhotonNetwork.automaticallySyncScene = true;
        ConnectToMaster();
    }

    public void ConnectToMaster()
    {
        //Debug.Log("@ LobbyManager::ConnectToMaster");
        if (!PhotonNetwork.connected)
        {
            PhotonNetwork.ConnectUsingSettings(_gameVersion);
        }
    }

    public override void OnConnectedToMaster()
    {
        //Debug.Log("@ LobbyManager::OnConnectedToMaster");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        //ChangePlayerName("New Player");
    }

    public override void OnJoinedLobby()
    {
        //Debug.Log("@ LobbyManager::OnJoinedLobby");
        ListRooms();
    }

    public void ListRooms()
    {
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        roomsGroup.UpdateRoomInfo(rooms);
        //Debug.Log("@ LobbyManager::ListRooms(" + rooms.Length + ")");
    }

    public void CreateRoom(string roomName, byte roomSize, int initialMoney)
    {
        //Debug.Log("@ LobbyManager::CreateRoom");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = roomSize;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        _initialMoney = initialMoney;
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        //Debug.Log("@ LobbyManager::OnCreatedRoom");
        if (PhotonNetwork.inRoom && PhotonNetwork.isMasterClient)
        {
            Hashtable customProps = new Hashtable();
            customProps["InitialMoney"] = _initialMoney;
            PhotonNetwork.room.SetCustomProperties(customProps);
        }
    }

    public void JoinRoom(string roomName)
    {
        //Debug.Log("@ LobbyManager::JoinRoom");
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        //Debug.Log("@ LobbyManager::OnJoinedRoom");
        roomInterface.SetActive(true);
    }

    public void LeaveRoom()
    {
        //Debug.Log("@ LobbyManager::LeaveRoom");
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        //Debug.Log("@ LobbyManager::OnLeftRoom");
        roomInterface.SetActive(false);
    }

    public void ChangePlayerName(string newName)
    {
        PhotonNetwork.player.NickName = newName;
        //Debug.Log("Player name has changed to " + PhotonNetwork.player.NickName);
    }
}
