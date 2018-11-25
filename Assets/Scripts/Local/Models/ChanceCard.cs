using System;

public class ChanceCard {

    string description;
    Action<Player> action;

    public ChanceCard(string description, Action<Player> action)
    {
        this.description = description;
        this.action = action;
    }

    public void Execute(Player callingPlayer)
    {
        action(callingPlayer);
    }
}
