using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

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
    [SerializeField]
    private Text warningMsgText;

    RoomInfo currentRoomInfo;

    // refresh button rotation animation
    private float refreshRotationTime = 0.5f;
    private float rotationTimeElasped = 0f;
    [SerializeField]
    private Image refreshButtonImg;


    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnCreateButtonClicked()
    {
        if (PhotonNetwork.player.NickName == "")
        {
            warningMsgText.text = "Please enter your player name first!";
            return;
        }
        warningMsgText.text = "";
        Instantiate(createRoomDialogPrefab, canvas, false);
    }

    public void OnJoinButtonClicked()
    { 
        if (currentRoomInfo != null)
        {
            if (PhotonNetwork.player.NickName == "")
            {
                warningMsgText.text = "Please enter your player name first!";
                return;
            }
            warningMsgText.text = "";
            LobbyManager.instance.JoinRoom(currentRoomInfo.Name);
            currentRoomInfo = null;
        }
    }

    public void OnRefreshButtonClicked()
    {
        StartCoroutine(RefreshButtonRotate());
        LobbyManager.instance.ListRooms();
    }

    IEnumerator RefreshButtonRotate()
    {
        while(rotationTimeElasped <= refreshRotationTime)
        {
            refreshButtonImg.transform.Rotate(0, 0, -Time.deltaTime / refreshRotationTime * 180);
            rotationTimeElasped += Time.deltaTime;
            yield return null;
        }
        rotationTimeElasped = 0;
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
