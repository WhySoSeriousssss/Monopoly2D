using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocGameManager : MonoBehaviour {

    #region Singleton
    public static LocGameManager instance;
    private void Awake()
    {
        if (instance != null)
            Debug.Log("Multiple LocPlayerController. Something went wrong");
        instance = this;
    }
    #endregion

    public static int initialMoney = 3500;
    public static int passByGoBonus = 200;
    public static int currentPlayerNum = 0;

    int numPlayers;
    List<Player> players;

    public bool debugOn;

    public delegate void OnCurrentPlayerChanged(Player player);
    public OnCurrentPlayerChanged OnCurrentPlayerChangedCallBack;


	// Use this for initialization
	void Start () {
        BoardManager.instance.Initialize();
        LocPlayerManager.instance.Initialize();
        //LocPlayerManager.instance.DebugInitialize();
        numPlayers = LocPlayerManager.numPlayers;
        players = LocPlayerManager.players;

        ChanceNChestManager.Instance.Initialize();
        SpinningWheelManager.instance.Initialize();

        StartGame();
    }
	
    void StartGame()
    {
        StartCoroutine(WaitForPlayerFinishTurn());
    }


    IEnumerator WaitForPlayerFinishTurn()
    {
        while (true)
        {
            Player currentPlayer = players[currentPlayerNum];
            OnCurrentPlayerChangedCallBack.Invoke(currentPlayer);
            currentPlayer.StartTurn();
            while (!currentPlayer.HasFinished)
            {
                yield return null;
            }
            if (!LocPlayerController.instance.AdditionalDice || currentPlayer.IsInJail)
                currentPlayerNum = (currentPlayerNum + 1) % numPlayers;
        }
    }

    public static void SetInitialMoney(int value)
    {
        initialMoney = value;
    }
}
