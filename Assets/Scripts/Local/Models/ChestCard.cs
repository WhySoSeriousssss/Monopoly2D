using System;

public class ChestCard {

    string description;
    Action<Player> action;

    public ChestCard(string description, Action<Player> action)
    {
        this.description = description;
        this.action = action;
    }

    public void Execute(Player callingPlayer)
    {
        action(callingPlayer);
    }
}
