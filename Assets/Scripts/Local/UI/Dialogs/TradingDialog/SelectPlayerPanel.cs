using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlayerPanel : MonoBehaviour {

    public GameObject playerButton;
    public Transform playerList;

    public void Initialize(Player callingPlayer)
    {
        List<Player> players = LocPlayerManager.players;
        foreach(Player p in players)
        {
            if (p != callingPlayer)
            {
                GameObject button = Instantiate(playerButton);
                button.transform.SetParent(playerList, false);
                button.GetComponentInChildren<Text>().text = p.PlayerName;
                button.GetComponent<TradedPlayerButton>().Initialize(p);
            }
        }
    }
}
