using System;

public class NChestCard {

    string _description;
    Action<NPlayer> _action;

    public NChestCard(string description, Action<NPlayer> action)
    {
        _description = description;
        _action = action;
    }

    public void Execute(NPlayer callingPlayer)
    {
        _action(callingPlayer);
    }
}
