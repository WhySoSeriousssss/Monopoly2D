
public class NChance : NSpace {

    public override void StepOn(NPlayer player)
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        NChestChanceManager.instance.ExecuteRandomChance(player);
    }
}
