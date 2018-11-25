using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parking : Space {

    int totalFee = 0;

    public void ReceiveFee(int amount)
    {
        totalFee += amount;
    }

    public override void StepOn(Player player)
    {
        player.GetMoney(totalFee);
        Debug.Log("player " + player.PlayerName + " gained parking fee $" + totalFee);
        totalFee = 0;
    }
}
