﻿
public class NChest : NSpace {

    public override void StepOn(NPlayer player)
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        NChestChanceManager.instance.ExecuteRandomChest(player);
    }
}
