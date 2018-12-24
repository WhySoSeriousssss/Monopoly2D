using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NGameplay : Photon.PunBehaviour
{
    #region Singleton
    public static NGameplay instance;
    private void Awake()
    {
        if (instance != null)
            Debug.Log("Multiple NGameplay. Something went wrong");
        instance = this;
    }
    #endregion


    private int _initialMoney = 3500;
    public static int passGoBonus = 200;

    public static int currentPlayerOrder = 0;
    private bool currentPlayerFinished = false;

    private bool _additionalDice = false;
    public bool AdditionalDice { get { return _additionalDice; } set { _additionalDice = value; } }

    public static bool _isDebug = true;

    int numPlayers;
    private NPlayer[] _players;
    public NPlayer[] Players{ get { return _players; } }

    Vector3 startPos = new Vector3(4, -4, 0);
    private static Vector3[] spawnPosOffsets = {
        new Vector3(0.26f, 0.05f, -1),
        new Vector3(-0.2f, -0.25f, -1),
        new Vector3(-0.13f, 0.15f, -1),
        new Vector3(0.12f, -0.25f, -1),
        new Vector3(0, 0, -1),
        new Vector3(0.2f, 0, -1)
    };

    public delegate void OnCurrenPlayerChanged(int playerNum);
    public OnCurrenPlayerChanged OnCurrentPlayerChangedCallBack;

    public delegate void OnMyTurnStarted();
    public OnMyTurnStarted OnMyTurnStartedCallback;

    private bool _startLocalInit = false;


    private void Start()
    {
        if (PhotonNetwork.inRoom)
        {
            _initialMoney = (int)PhotonNetwork.room.CustomProperties["InitialMoney"];
            numPlayers = PhotonNetwork.playerList.Length;
            _players = FindObjectsOfType<NPlayer>();
            
            // Master Client side set up
            if (PhotonNetwork.isMasterClient)
            {
                /*
                // TO-DO:shuffle the players and assign them random orders
                for (int i = roomPlayers.Length - 1; i >= 0; i--)
                {
                    int index = Random.Range(0, i);
                    RoomPlayer temp = roomPlayers[index];
                    roomPlayers[index] = roomPlayers[i];
                    roomPlayers[i] = temp;
                }
                */

                for (int i = 0; i < _players.Length; i++)
                {
                    Array.Find(_players, x => x.photonView.owner == PhotonNetwork.playerList[i]).SetOrder(i);
                    _players[i].SetMoney(_initialMoney);
                    _players[i].SetTokenPosition(startPos + spawnPosOffsets[i]);
                    _players[i].SetColor((int)_players[i].photonView.owner.CustomProperties["Color"]);
                }
                photonView.RPC("RPC_SetStartLocalInitialization", PhotonTargets.All);
            }

            StartCoroutine(StartLocalInitialization());

        }
    }

    IEnumerator StartLocalInitialization()
    {
        while(!_startLocalInit)
        {
            yield return null;
        }
        // local client side set up
        NBoardManager.instance.Initialize();
        NPlayerInfoPanel.instance.Initialize(_players);
        NTurnButtonPanel.instance.Initialize();
        NDialogManager.instance.Initialize();
        NPlayer.Initialize();

        List<NPlayer> rearrangedPlayers = new List<NPlayer>();
        for (int i = 0; i < _players.Length; i++)
        {
            rearrangedPlayers.Add(Array.Find(_players, x => x.Order == i));
        }
        _players = rearrangedPlayers.ToArray();

        StartGame();
    }

    public void StartGame()
    {
        StartCoroutine(WaitForMyTurn());
    }

    IEnumerator WaitForMyTurn()
    {
        while (true)
        {
            if (PhotonNetwork.isMasterClient)
            {
                photonView.RPC("RPC_SetCanStart", _players[currentPlayerOrder].photonView.owner);

                if (currentPlayerOrder == NPlayer.thisPlayer.Order)
                {
                    OnMyTurnStartedCallback.Invoke();
                    NPlayer.thisPlayer.CanStart = false;
                    while (!NPlayer.thisPlayer.HasFinished)
                        yield return null;
                    currentPlayerFinished = true;
                    NPlayer.thisPlayer.HasFinished = false;
                }

                while (!currentPlayerFinished)
                    yield return null;
                currentPlayerFinished = false;

                if (!_additionalDice)
                    currentPlayerOrder = (currentPlayerOrder + 1) % numPlayers;
                else
                    _additionalDice = false;

                photonView.RPC("RPC_UpdateCurrentPlayerOrder", PhotonTargets.All, currentPlayerOrder);
            }
            else
            {
                while (!NPlayer.thisPlayer.CanStart)
                    yield return null;
                OnMyTurnStartedCallback.Invoke();
                NPlayer.thisPlayer.CanStart = false;
                while (!NPlayer.thisPlayer.HasFinished)
                    yield return null;
                photonView.RPC("RPC_CurrentPlayerFinished", PhotonTargets.MasterClient);
                NPlayer.thisPlayer.HasFinished = false;
            }
        }
    }

    [PunRPC]
    public void RPC_SetStartLocalInitialization()
    {
        _startLocalInit = true;
    }

    [PunRPC]
    public void RPC_UpdateCurrentPlayerOrder(int newOrder)
    {
        currentPlayerOrder = newOrder;
        OnCurrentPlayerChangedCallBack.Invoke(newOrder);
    }

    [PunRPC]
    public void RPC_SetCanStart()
    {
        NPlayer.thisPlayer.CanStart = true;
    }

    [PunRPC]
    public void RPC_CurrentPlayerFinished()
    {
        if (PhotonNetwork.isMasterClient)
            currentPlayerFinished = true;
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
