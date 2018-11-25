using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuctionSystem : MonoBehaviour {

    #region Singleton
    public static AuctionSystem instance;
    private void Awake()
    {
        if (instance != null)
            Debug.Log("Multiple PropertyPurchaseHandler. Something went wrong");
        instance = this;
    }
    #endregion

    List<Player> playersInAuction;
    int currentPlayerIndex;

    Property property;

    bool hasReceivedBid;
    bool bidOrStop;
    int currentBiddingPrice;

    AuctionDialog auctionDialog;



    public void Initialize(Player startingPlayer, Property propertyForAuction, AuctionDialog auctiondialog)
    {
        playersInAuction = new List<Player>();
        for (int i = 0; i < LocPlayerManager.numPlayers; i++)
            playersInAuction.Add(LocPlayerManager.players[i]);

        property = propertyForAuction;
        currentPlayerIndex = playersInAuction.IndexOf(startingPlayer);
        currentBiddingPrice = propertyForAuction.PurchasePrice;

        auctionDialog = auctiondialog;

        StartCoroutine(Bidding());
    }

    IEnumerator Bidding()
    {
        while(playersInAuction.Count > 1)
        {
            hasReceivedBid = false;

            while (!hasReceivedBid)
                yield return null;
            if (bidOrStop)
            {
                auctionDialog.UpdateNewBid(playersInAuction[currentPlayerIndex], currentBiddingPrice);
                currentPlayerIndex = (currentPlayerIndex + 1) % playersInAuction.Count;
            }
            else
            {
                auctionDialog.UpdateStopBid(playersInAuction[currentPlayerIndex]);
                playersInAuction.Remove(playersInAuction[currentPlayerIndex]);
                if (currentPlayerIndex == playersInAuction.Count)
                    currentPlayerIndex = 0;
            }
            
        }
        LocPlayerController.instance.WonAuction(playersInAuction[0], property, currentBiddingPrice);
        auctionDialog.AuctionFinish();
    }

    public void ReceiveBid(bool newBid, int bidPrice)
    {
        hasReceivedBid = true;
        bidOrStop = newBid;
        if (bidOrStop)
            currentBiddingPrice = bidPrice;
    }

}
