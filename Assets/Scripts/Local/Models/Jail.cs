using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jail : Space {

    int locationIndex = 10;
    public int LocationIndex { get { return locationIndex; } }

    public override void StepOn(Player player)
    {
        Debug.Log("player " + player.PlayerName + " steped on Jail");
    }
}
