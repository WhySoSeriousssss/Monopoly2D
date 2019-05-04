using UnityEngine;
using UnityEngine.UI;

public class CreateRoomDialog : MonoBehaviour {

    [SerializeField]
    private InputField roomNameInput;
    [SerializeField]
    private Dropdown numPlayersDropDown;
    [SerializeField]
    private Dropdown initialMoneyDropDown;
    [SerializeField]
    private Text warningMsgLabel;


    public void OnCreateButtonOnClicked()
    {
        string roomName = roomNameInput.text;
        int numPlayers = int.Parse(numPlayersDropDown.captionText.text);
        int initialMoney = int.Parse(initialMoneyDropDown.captionText.text);

        if (roomName == "")
        {
            warningMsgLabel.text = "Room name cannot be empty!";
            return;
        }
        warningMsgLabel.text = "";
        LobbyManager.instance.CreateRoom(roomName, (byte)numPlayers, initialMoney);
        Destroy(gameObject);
    }

    public void OnCloseButtonOnClicked()
    {
        Destroy(gameObject);
    }
}
