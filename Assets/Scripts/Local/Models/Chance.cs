
public class Chance : Space {

    public override void StepOn(Player player)
    {
        ChanceNChestManager.Instance.ExecuteRandomChance(player);
    }
}
