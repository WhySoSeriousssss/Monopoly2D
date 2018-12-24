
public class NGoJail : NSpace {

    public override void StepOn(NPlayer player)
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        player.GoJail();
    }

}
