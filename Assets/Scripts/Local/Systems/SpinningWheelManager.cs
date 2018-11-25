using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpinningWheelManager : MonoBehaviour{

    #region Singleton
    public static SpinningWheelManager instance;
    private void Awake()
    {
        if (instance != null)
            Debug.Log("Multiple SpinningWheelManager. Something went wrong");
        instance = this;
    }
    #endregion


    List<Action<Player>> wheelItems = new List<Action<Player>>();

    public GameObject spinningWheelPrefab;
    public Transform HUD;


    public void Initialize()
    {   
        wheelItems.Add(Wheel_GetOneHundredDollars);
        wheelItems.Add(Wheel_DegradeLand);
        wheelItems.Add(Wheel_GetFiveHundredDollars);
        wheelItems.Add(Wheel_MortgageProperty);
        wheelItems.Add(Wheel_giveOneHundredToOtherPlayers);
        wheelItems.Add(Wheel_receiveOneHundredFromOtherPlayers);
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

    public void InvokeWheel(Player invoker)
    {
        GameObject wheelObject = Instantiate(spinningWheelPrefab, HUD, false);
        SpinWheel wheel = wheelObject.GetComponent<SpinWheel>();
        wheel.Initialize(invoker);
    }

    public void Execute(int itemIndex, Player player)
    {
        wheelItems[itemIndex](player);
    }

    // 1
    void Wheel_GetOneHundredDollars(Player player)
    {
        player.GetMoney(100);
    }

    // 2
    void Wheel_LoseOneHundredDollars(Player player)
    {
        player.LoseMoney(100);
    }

    // 3
    void Wheel_GetTwoHundredDollars(Player player)
    {
        player.GetMoney(200);
    }

    // 4
    void Wheel_LoseTwoHundredDollars(Player player)
    {
        player.LoseMoney(200);
    }

    // 5
    void Wheel_GetFiveHundredDollars(Player player)
    {
        player.GetMoney(500);
    }

    // 6
    void Wheel_LoseTwentyPercentMoney(Player player)
    {
        player.LoseMoney((int)(player.CurrentMoney * 0.2f));
    }

    // 7
    void Wheel_MoneyBecomesOneThousand(Player player)
    {
        player.CurrentMoney = 1000;
    }

    // 8
    void Wheel_receiveOneHundredFromOtherPlayers(Player player)
    {
        int numOtherPlayers = LocPlayerManager.players.Count - 1;
        foreach (Player p in LocPlayerManager.players)
        {
            p.LoseMoney(100 / numOtherPlayers);
        }
        player.GetMoney(100);
    }

    // 9
    void Wheel_giveOneHundredToOtherPlayers(Player player)
    {
        int numOtherPlayers = LocPlayerManager.players.Count - 1;
        foreach (Player p in LocPlayerManager.players)
        {
            p.GetMoney(100 / numOtherPlayers);
        }
        player.LoseMoney(100);
    }

    // 10
    void Wheel_GoToJail(Player player)
    {
        Jail jail = FindObjectOfType<Jail>();
        player.GetIntoJail(jail);
    }

    // 11
    void Wheel_UpgradeLand(Player player)
    {

    }

    // 12
    void Wheel_DegradeLand(Player player)
    {

    }

    // 13
    void Wheel_MortgageProperty(Player player)
    {

    }

    // 14
    void Wheel_RedeemProperty(Player player)
    {

    }

    // 15
    void Wheel_GoToParkingLot(Player player)
    {

    }

    // 16
    void Wheel_GetPropertyForFree(Player player)
    {
        
    }
}
