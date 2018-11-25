using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradingDialog : MonoBehaviour {

    public Transform selectPlayerPanel;
    public Transform tradingPanel;

    Player callingPlayer;

    Player tradedPlayer;
    public Player TradedPlayer { set { tradedPlayer = value; } }

    public void Initialize(Player callingPlayer)
    {
        tradedPlayer = null;
        this.callingPlayer = callingPlayer;
        selectPlayerPanel.GetComponent<SelectPlayerPanel>().Initialize(callingPlayer);
        StartCoroutine(WaitForSelectingTradedPlayer());
    }

    IEnumerator WaitForSelectingTradedPlayer()
    {
        while (tradedPlayer == null)
            yield return null;
        selectPlayerPanel.gameObject.SetActive(false);
        tradingPanel.gameObject.SetActive(true);
        tradingPanel.GetComponent<TradingPanel>().Initialize(callingPlayer, tradedPlayer);
    }
}
