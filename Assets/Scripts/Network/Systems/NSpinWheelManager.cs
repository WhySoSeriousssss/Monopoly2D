using System;
using System.Collections.Generic;
using UnityEngine;

public class NSpinWheelManager : Photon.MonoBehaviour {

    #region Singleton
    public static NSpinWheelManager instance;
    private void Awake()
    {
        if (instance != null)
            Debug.Log("Multiple NSpinWheelManager. Something went wrong");
        instance = this;
    }
    #endregion


    List<Action<NPlayer>> wheelItems = new List<Action<NPlayer>>();

    public GameObject spinWheelPrefab;
    public Transform HUD;

    NPlayer _invoker;


    private void Start()
    {
        wheelItems.Add(Wheel_GetOneHundredDollars);
        wheelItems.Add(Wheel_DegradeLand);
        wheelItems.Add(Wheel_GetFiveHundredDollars);
        wheelItems.Add(Wheel_MortgageProperty);
        wheelItems.Add(Wheel_giveOneHundredToOtherPlayer);
        wheelItems.Add(Wheel_receiveOneHundredFromOtherPlayer);
        wheelItems.Add(Wheel_MoneyBecomesOneThousand);
        wheelItems.Add(Wheel_UpgradeLand);
        wheelItems.Add(Wheel_GetPropertyForFree);
        wheelItems.Add(Wheel_GoToParkingLot);
        wheelItems.Add(Wheel_LoseTwoHundredDollars);
        wheelItems.Add(Wheel_LoseTwentyPercentMoney);
        wheelItems.Add(Wheel_GetTwoHundredDollars);
        wheelItems.Add(Wheel_RedeemProperty);
        wheelItems.Add(Wheel_LoseOneHundredDollars);
        wheelItems.Add(Wheel_GoToJail);
    }

    public void InvokeWheel(NPlayer invoker)
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        _invoker = invoker;
        float angularSpeed = 260 + UnityEngine.Random.Range(-30f, 30f);
        float angularAcceleration = 40f + UnityEngine.Random.Range(-5f, 5f);

        photonView.RPC("RPC_InvokeWheel", PhotonTargets.All, angularSpeed, angularAcceleration);
    }

    [PunRPC]
    public void RPC_InvokeWheel(float angularSpeed, float angularAcceleration)
    {
        GameObject wheelObject = Instantiate(spinWheelPrefab, HUD, false);
        NSpinWheel wheel = wheelObject.GetComponent<NSpinWheel>();
        wheel.Spin(angularSpeed, angularAcceleration);
    }


    public void Execute(int itemIndex)
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        if (_invoker != null)
            wheelItems[itemIndex](_invoker);
        _invoker = null;
    }

    // 1
    void Wheel_GetOneHundredDollars(NPlayer player)
    {
        player.ChangeMoney(100);
    }

    // 2
    void Wheel_LoseOneHundredDollars(NPlayer player)
    {
        player.ChangeMoney(-100);
    }

    // 3
    void Wheel_GetTwoHundredDollars(NPlayer player)
    {
        player.ChangeMoney(200);
    }

    // 4
    void Wheel_LoseTwoHundredDollars(NPlayer player)
    {
        player.ChangeMoney(-200);
    }

    // 5
    void Wheel_GetFiveHundredDollars(NPlayer player)
    {
        player.ChangeMoney(500);
    }

    // 6
    void Wheel_LoseTwentyPercentMoney(NPlayer player)
    {
        player.ChangeMoney((int)(-player.CurrentMoney * 0.2f));
    }

    // 7
    void Wheel_MoneyBecomesOneThousand(NPlayer player)
    {
        player.SetMoney(1000);
    }

    // 8
    void Wheel_receiveOneHundredFromOtherPlayer(NPlayer player)
    {
        NPlayer[] players = NGameplay.instance.Players;
        int max = 0;
        int id = 0;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == player)
                continue;
            if (players[i].CurrentMoney > max)
            {
                max = players[i].CurrentMoney;
                id = i;
            }
        }
        players[id].ChangeMoney(-100);
        player.ChangeMoney(100);
    }

    // 9
    void Wheel_giveOneHundredToOtherPlayer(NPlayer player)
    {
        NPlayer[] players = NGameplay.instance.Players;
        int min = 100000;
        int id = 0;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == player)
                continue;
            if (players[i].CurrentMoney < min)
            {
                min = players[i].CurrentMoney;
                id = i;
            }
        }
        players[id].ChangeMoney(100);
        player.ChangeMoney(-100);
    }

    // 10
    void Wheel_GoToJail(NPlayer player)
    {
        //Jail jail = FindObjectOfType<Jail>();
        //player.GetIntoJail(jail);
    }

    // 11
    void Wheel_UpgradeLand(NPlayer player)
    {

    }

    // 12
    void Wheel_DegradeLand(NPlayer player)
    {

    }

    // 13
    void Wheel_MortgageProperty(NPlayer player)
    {

    }

    // 14
    void Wheel_RedeemProperty(NPlayer player)
    {

    }

    // 15
    void Wheel_GoToParkingLot(NPlayer player)
    {

    }

    // 16
    void Wheel_GetPropertyForFree(NPlayer player)
    {

    }



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
