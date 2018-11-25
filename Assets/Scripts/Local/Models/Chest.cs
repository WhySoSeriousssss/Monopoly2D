
public class Chest : Space {

    public override void StepOn(Player player)
    {
        ChanceNChestManager.Instance.ExecuteRandomChest(player);
    }

}
