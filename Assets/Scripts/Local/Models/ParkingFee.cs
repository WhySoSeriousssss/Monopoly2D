using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingFee : Space {

    int fee = 100;
    Parking parking;

    private void Start()
    {
        parking = FindObjectOfType<Parking>();
        if (parking == null)
            Debug.Log("couldn't find Parking");
    }

    public override void StepOn(Player player)
    {
        player.LoseMoney(fee);
        parking.ReceiveFee(fee);
        Debug.Log("Player " + player.PlayerName + " paid parking fee $" + fee);
    }
}
