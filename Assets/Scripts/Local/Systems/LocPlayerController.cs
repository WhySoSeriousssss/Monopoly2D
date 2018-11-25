using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocPlayerController : MonoBehaviour {

    #region Singleton
    public static LocPlayerController instance;
    private void Awake()
    {
        if (instance != null)
            Debug.Log("Multiple MonopolyManager. Something went wrong");
        instance = this;
    }
    #endregion


    List<Space> spaces;
    int numSpaces;


    Player currentPlayer;

    int dice1;
    int dice2;
    int steps;
    bool additionalDice = false;
    public bool AdditionalDice { get { return additionalDice; } }

    private void Start()
    {
        spaces = BoardManager.instance.spaces;
        numSpaces = BoardManager.numSpaces;

        LocGameManager.instance.OnCurrentPlayerChangedCallBack += SetCurrentPlayer;
    }

    void SetCurrentPlayer(Player player)
    {
        dice1 = dice2 = steps = 0;
        currentPlayer = player;
        additionalDice = false;

        if (currentPlayer.IsInJail && currentPlayer.TurnsInJail < 3)
        {
            DialogHandler.instance.CallInJailDialog(currentPlayer);
        }
    }

    public void ClickRollButton()
    {
        dice1 = Random.Range(1, 6);
        dice2 = Random.Range(1, 6);
        Debug.Log("Player " + currentPlayer.PlayerName + " has rolled " + (dice1 + dice2));
        steps = dice1 + dice2;
        if (dice1 == dice2)
            additionalDice = true;

        StartCoroutine(currentPlayer.Move(steps));
    }

    // debug
    public void ReceiveDicePoint(int value, bool additionalRoll)
    {
        steps = value;
        additionalDice = additionalRoll;
        StartCoroutine(currentPlayer.Move(steps));
    }


    public void TryingToBuyProperty(Property property)
    {
        if (currentPlayer.CurrentMoney > property.PurchasePrice)
        {
            currentPlayer.LoseMoney(property.PurchasePrice);
            currentPlayer.OwnProperty(property);
            property.SoldTo(currentPlayer);
        }
    }

    public void AuctionProperty(Property property)
    {
        AuctionDialog ad = DialogHandler.instance.CallAuctionDialog(currentPlayer, property);
        AuctionSystem.instance.Initialize(currentPlayer, property, ad);
    }

    public void WonAuction(Player player, Property property, int price)
    {
        player.LoseMoney(price);
        player.OwnProperty(property);
        property.SoldTo(player);
        Debug.Log("Player " + player.PlayerName + " won " + property.PropertyName + " for $" + price);
    }

    public void PassByGO()
    {
        currentPlayer.GetMoney(LocGameManager.passByGoBonus);
        Debug.Log("player " + currentPlayer.PlayerName + " passed by GO and received $" + LocGameManager.passByGoBonus);
    }
    

    public void BailToRelease()
    {
        currentPlayer.LoseMoney(100);
        currentPlayer.GetOutOfJail();
    }

    public void RollDiceToRelease()
    {
        dice1 = Random.Range(1, 6);
        dice2 = Random.Range(1, 6);
        Debug.Log(currentPlayer.PlayerName + " rolled " + dice1 + " + " + dice2);
        if (dice1 == dice2)
        {
            currentPlayer.GetOutOfJail();
            steps = dice1 + dice2;
            StartCoroutine(currentPlayer.Move(steps));
        }
        else
        {
            currentPlayer.TurnsInJail++;
            TurnButtonPanel.instance.ToggleButtons(true);
        }
    }

    public void UseCardToRelease()
    {
        /* wait for implementation */
    }

}
