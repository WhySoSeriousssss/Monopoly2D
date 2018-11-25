using UnityEngine;

public class GoToJail : Space {

    Jail jail;

    private void Start()
    {
        jail = FindObjectOfType<Jail>();
    }

    public override void StepOn(Player player)
    {
        player.GetIntoJail(jail);
        Debug.Log("Player " + player.PlayerName + " went to jail");
    }
}
