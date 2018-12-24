using System;

public class NChanceCard {

    string _description;
    Action<NPlayer> _action;

    public NChanceCard(string description, Action<NPlayer> action)
    {
        _description = description;
        _action = action;
    }

    public void Execute(NPlayer callingPlayer)
    {
        _action(callingPlayer);
    }
}
