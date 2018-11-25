using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPlayerSlot : MonoBehaviour {

    [SerializeField]
    public InputField playerNameInput;
    [SerializeField]
    public Text readyStatusText;
    [SerializeField]
    public Dropdown colorDropDown;
    [SerializeField]
    public Button readyButton;


    //static List<Color> unusedColors = new List<Color>();


    public void Initialize(RoomPlayer newPlayer)
    {
        readyButton.onClick.AddListener(newPlayer.ToggleReadyStatus);
        //playerNameInput.onEndEdit.AddListener(newPlayer.ChangePlayerName);
        colorDropDown.onValueChanged.AddListener(newPlayer.ChangeColor);

        SetPlayerName(newPlayer.PhotonPlayer.NickName);
        SetReadyStatus(false);
        SetColor(0);
    }

    public void SetAsLocal()
    {
        readyButton.gameObject.SetActive(true);
        colorDropDown.interactable = true;
        //playerNameInput.interactable = true;
    }
    

    public void SetPlayerName(string name)
    {
        playerNameInput.text = name;
    }
    
    public void SetReadyStatus(bool ready)
    {
        readyStatusText.text = (ready ? "Ready" : "Not Ready");
    }

    public void SetColor(int colorIndex)
    {
        colorDropDown.value = colorIndex;
    }

}
