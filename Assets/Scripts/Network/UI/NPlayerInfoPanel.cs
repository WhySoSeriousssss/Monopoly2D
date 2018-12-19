using System;
using System.Collections.Generic;
using UnityEngine;

public class NPlayerInfoPanel : MonoBehaviour {

    #region Singleton
    public static NPlayerInfoPanel instance;
    private void Awake()
    {
        if (instance != null)
            Debug.Log("Multiple NPlayerInfoPanel. Something went wrong");
        instance = this;
    }
    #endregion

    [SerializeField]
    public GameObject playerInfoPrefab;

    List<NPlayerInfo> playersInfo = new List<NPlayerInfo>();


    public void Initialize(NPlayer[] players)
    {
        for (int i = 0; i < players.Length; i++)
        {
            GameObject newPlayerInfo = Instantiate(playerInfoPrefab, transform, false);
            NPlayerInfo playerInfo = newPlayerInfo.GetComponent<NPlayerInfo>();
            playerInfo.Initialize(Array.Find(players, x => x.Order == i));
            playersInfo.Add(playerInfo);
        }

        // hightlight the first player info
        playersInfo[0].ToggleHighlighted(true);

        NGameplay.instance.OnCurrentPlayerChangedCallBack += SetCurrentPlayer;
    }

    public void SetCurrentPlayer(int playerNum)
    {
        int previousPlayer = (playerNum + playersInfo.Count - 1) % playersInfo.Count;
        playersInfo[previousPlayer].ToggleHighlighted(false);
        playersInfo[playerNum].ToggleHighlighted(true);
    }
}
