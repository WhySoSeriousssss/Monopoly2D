using System.Collections.Generic;
using UnityEngine;

public class ChanceNChestManager {

    #region Singleton
    private static ChanceNChestManager instance;
    public static ChanceNChestManager Instance
    {
        get
        {
            if (instance == null)
                instance = new ChanceNChestManager();
            return instance;
        }
    }
    #endregion

    List<ChestCard> chestList = new List<ChestCard>();
    List<ChanceCard> chanceList = new List<ChanceCard>();

    public void Initialize()
    {
        chestList.Add(new ChestCard("chest description1", Chest_1));
        chestList.Add(new ChestCard("chest description2", Chest_2));
        chestList.Add(new ChestCard("chest description3", Chest_3));

        chanceList.Add(new ChanceCard("chance description1", Chance_1));
        chanceList.Add(new ChanceCard("chance description2", Chance_2));
        chanceList.Add(new ChanceCard("chance description3", Chance_3));
    }

    public void ExecuteRandomChance(Player callingPlayer)
    {
        int index = Random.Range(0, chanceList.Count);
        chanceList[index].Execute(callingPlayer);
    }

    public void ExecuteRandomChest(Player callingPlayer)
    {
        int index = Random.Range(0, chestList.Count);
        chestList[index].Execute(callingPlayer);
    }

    void Chance_1(Player callingPlayer)
    {
        Debug.Log(callingPlayer.PlayerName + " chance 1");
    }

    void Chance_2(Player callingPlayer)
    {
        Debug.Log(callingPlayer.PlayerName + " chance 2");
    }

    void Chance_3(Player callingPlayer)
    {
        Debug.Log(callingPlayer.PlayerName + " chance 3");
    }

    void Chest_1(Player callingPlayer)
    {
        Debug.Log(callingPlayer.PlayerName + " chest 1");
    }

    void Chest_2(Player callingPlayer)
    {
        Debug.Log(callingPlayer.PlayerName + " chest 2");
    }

    void Chest_3(Player callingPlayer)
    {
        Debug.Log(callingPlayer.PlayerName + " chest 3");
    }
}
