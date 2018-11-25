using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogHandler : MonoBehaviour {

    #region Singleton
    public static DialogHandler instance;
    private void Awake()
    {
        if (instance != null)
            Debug.Log("Multiple PropertyPurchaseHandler. Something went wrong");
        instance = this;
    }
    #endregion

    public GameObject propertyPurchaseDialogPrefab;
    public GameObject inJailDialogPrefab;
    public GameObject tradingDialogPrefab;
    public GameObject auctionDialogPrefab;

    public void CallPropertyPurchaseWindow(Property propertyToSell)
    {
        GameObject dialog = Instantiate(propertyPurchaseDialogPrefab);
        dialog.GetComponent<PropertyPurchaseDialog>().Initialize(propertyToSell);
    }

    public AuctionDialog CallAuctionDialog(Player callingPlayer, Property propertyToAuction)
    {
        GameObject dialog = Instantiate(auctionDialogPrefab);
        AuctionDialog ad = dialog.GetComponent<AuctionDialog>();
        ad.Initialize(callingPlayer, propertyToAuction);
        return ad;
    }

    public void CallInJailDialog(Player player)
    {
        Instantiate(inJailDialogPrefab);
    }

    public void CallTradingDialog(Player callingPlayer)
    {
        GameObject dialog = Instantiate(tradingDialogPrefab);
        dialog.GetComponent<TradingDialog>().Initialize(callingPlayer);
    }

    
}
