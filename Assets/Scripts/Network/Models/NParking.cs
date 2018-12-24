
public class NParking : NSpace {

    int totalFee = 0;
    // TextMesh text;

    public void ReceiveFee(int amount)
    {
        totalFee += amount;
        // text.text = "$" + amount.ToString();
    }


    public override void StepOn(NPlayer player)
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        player.ChangeMoney(totalFee);
        totalFee = 0;
        // text.text = "";
    }

}
