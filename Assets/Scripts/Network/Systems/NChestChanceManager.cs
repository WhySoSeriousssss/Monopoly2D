using System.Collections.Generic;
using UnityEngine;

public class NChestChanceManager : MonoBehaviour {

    #region Singleton
    public static NChestChanceManager instance;
    private void Awake()
    {
        if (instance != null)
            Debug.Log("Multiple NChestChanceManager. Something went wrong");
        instance = this;
    }
    #endregion


    List<NChestCard> chestList = new List<NChestCard>();
    List<NChanceCard> chanceList = new List<NChanceCard>();


    private void Start()
    {
        chestList.Add(new NChestCard("chest 1 description", Chest_1));
        chestList.Add(new NChestCard("chest 2 description", Chest_2));
        chestList.Add(new NChestCard("chest 3 description", Chest_3));

        chanceList.Add(new NChanceCard("chance 1 description", Chance_1));
        chanceList.Add(new NChanceCard("chance 2 description", Chance_2));
        chanceList.Add(new NChanceCard("chance 3 description", Chance_3));
    }

    public void ExecuteRandomChance(NPlayer callingPlayer)
    {
        int index = Random.Range(0, chanceList.Count);
        chanceList[index].Execute(callingPlayer);
    }

    public void ExecuteRandomChest(NPlayer callingPlayer)
    {
        int index = Random.Range(0, chestList.Count);
        chestList[index].Execute(callingPlayer);
    }

    void Chance_1(NPlayer callingPlayer)
    {
        Debug.Log(callingPlayer.photonView.owner.NickName + " chance 1");
    }

    void Chance_2(NPlayer callingPlayer)
    {
        Debug.Log(callingPlayer.photonView.owner.NickName + " chance 2");
    }

    void Chance_3(NPlayer callingPlayer)
    {
        Debug.Log(callingPlayer.photonView.owner.NickName + " chance 3");
    }

    void Chest_1(NPlayer callingPlayer)
    {
        Debug.Log(callingPlayer.photonView.owner.NickName + " chest 1");
    }

    void Chest_2(NPlayer callingPlayer)
    {
        Debug.Log(callingPlayer.photonView.owner.NickName + " chest 2");
    }

    void Chest_3(NPlayer callingPlayer)
    {
        Debug.Log(callingPlayer.photonView.owner.NickName + " chest 3");
    }
}
