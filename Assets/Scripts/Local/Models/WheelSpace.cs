using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSpace : Space {

    public override void StepOn(Player player)
    {
        SpinningWheelManager.instance.InvokeWheel(player);
    }
}
