
public class NParkingFee : NSpace {
    int fee = 100;

    public override void StepOn(NPlayer player)
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        player.ChangeMoney(-fee);
        FindObjectOfType<NParking>().ReceiveFee(fee);
    }
}
