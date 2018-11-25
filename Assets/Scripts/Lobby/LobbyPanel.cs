using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class LobbyPanel : MonoBehaviour {

    #region Singleton
    public static LobbyPanel instance;
    private void Awake()
    {
        if (instance != null)
            Debug.Log("Multiple MonopolyLobby. Something went wrong");
        instance = this;
    }
    #endregion

    [SerializeField]
    private GameObject createRoomDialogPrefab;
    [SerializeField]
    private Transform canvas;
    [SerializeField]
    private InputField playerNameInput;
    
    RoomInfo currentRoomInfo;


    public void OnCreateButtonClicked()
    {
        Instantiate(createRoomDialogPrefab, canvas, false);
    }

    public void OnJoinButtonClicked()
    {
        if (currentRoomInfo != null)
        {
            LobbyManager.instance.JoinRoom(currentRoomInfo.Name);
            currentRoomInfo = null;
        }
    }

    public void OnRefreshButtonClicked()
    {
        LobbyManager.instance.ListRooms();
    }

    public void NewRoomSelected(RoomInfo newRoomInfo)
    {
        currentRoomInfo = newRoomInfo;
    }

    public void OnPlayerNameEdited(string newName)
    {
        LobbyManager.instance.ChangePlayerName(newName);
    }
}
