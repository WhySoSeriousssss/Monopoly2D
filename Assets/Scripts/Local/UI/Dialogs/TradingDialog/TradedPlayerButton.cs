using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradedPlayerButton : MonoBehaviour {

    Player player;

    public void Initialize(Player player)
    {
        this.player = player;
    }

	public void ButtonOnClick()
    {
        GetComponentInParent<TradingDialog>().TradedPlayer = player;
    }
}
