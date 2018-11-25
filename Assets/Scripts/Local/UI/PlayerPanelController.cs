using System.Collections.Generic;
using UnityEngine;

public class PlayerPanelController : MonoBehaviour {

    #region Singleton
    public static PlayerPanelController instance;
    private void Awake()
    {
        if (instance != null)
            Debug.Log("Multiple PlayerPanelController. Something went wrong");
        instance = this;
    }
    #endregion

    public GameObject playerCardPrefab;

    Dictionary<Player, PlayerCardController> playerCards = new Dictionary<Player, PlayerCardController>();

    Player currentPlayer  = null;


    public void Initialize(List<Player> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            GameObject newPlayerCard = Instantiate(playerCardPrefab);
            newPlayerCard.transform.SetParent(transform, false);

            playerCards.Add(players[i], newPlayerCard.GetComponent<PlayerCardController>());
            playerCards[players[i]].Initialize(players[i]);
        }

        LocGameManager.instance.OnCurrentPlayerChangedCallBack += SetCurrentPlayer;
    }

    public void SetCurrentPlayer(Player player)
    {
        if (currentPlayer != null)
            playerCards[currentPlayer].ToggleHighlighted();
        currentPlayer = player;
        playerCards[currentPlayer].ToggleHighlighted();
    }    
}
