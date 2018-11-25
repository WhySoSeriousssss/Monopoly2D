using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LocalGameSettingPanel : MonoBehaviour {

    public Transform[] playerInfoPanels;
    public Dropdown numPlayersDropDown;
    public Dropdown initMoneyDropDown;

    int numPlayers;

    Dictionary<string, Color> playerColors = new Dictionary<string, Color>();

    private void Start()
    {
        // initialize the colors
        playerColors.Add("Red", Color.red);
        playerColors.Add("Yellow", Color.yellow);
        playerColors.Add("Blue", Color.blue);
        playerColors.Add("Grey", Color.grey);
        playerColors.Add("Green", Color.green);
        playerColors.Add("Cyan", Color.cyan);
        playerColors.Add("Magenta", Color.magenta);

        // set the initial player info input panel
        numPlayers = numPlayersDropDown.value + 2;
        for (int i = 0; i < numPlayers; i++)
        {
            playerInfoPanels[i].gameObject.SetActive(true);
        }

        // initialize the color selection  drop down
        foreach (Transform playerInfo in playerInfoPanels)
        {
            PlayerInfoPanel pip = playerInfo.GetComponent<PlayerInfoPanel>();
            List<string> keys = new List<string>();
            foreach (string key in playerColors.Keys)
            {
                keys.Add(key);
            }
            pip.SetColorDropDown(keys);
        }       
    }

    public void NumPlayerDropDownOnValueChanged(int value)
    {
        numPlayers = value + 2;
        int i = 0;
        for (; i < numPlayers; i++)
        {
            playerInfoPanels[i].gameObject.SetActive(true);
        }
        for (; i < 6; i++)
        {
            playerInfoPanels[i].gameObject.SetActive(false);
        }
    }

    public void StartButtonOnClicked()
    {
        string[] names = new string[numPlayers];
        Color[] colors = new Color[numPlayers];

        for (int i = 0; i < numPlayers; i++)
        {
            string nameString = playerInfoPanels[i].GetComponent<PlayerInfoPanel>().GetInputPlayerName();
            string colorString = playerInfoPanels[i].GetComponent<PlayerInfoPanel>().GetSelectedColorString();
            if (nameString == "" || colorString == "")
            {
                Debug.Log("Empty name or color");
                return;
            }
            names[i] = nameString;
            colors[i] = playerColors[colorString];
        }

        LocGameManager.SetInitialMoney(int.Parse(initMoneyDropDown.captionText.text));
        LocPlayerManager.ReceivePlayersData(names, colors);

        SceneManager.LoadScene("MainGameLocal");

        Destroy(gameObject);
    }
}
