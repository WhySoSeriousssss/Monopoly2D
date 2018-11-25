using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPropertyPurchaseDialog : NDialog {

    [SerializeField]
    private GameObject landCardPrefab;
    [SerializeField]
    private GameObject railroadCardPrefab;
    [SerializeField]
    private Transform propertyCardPlaceholder;
    [SerializeField]
    private Button buyButton;
    [SerializeField]
    private Button auctionButton;

    NProperty _property;

    
    public void Initialize(NProperty propertyToSell, bool isCaller)
    {
        _property = propertyToSell;
        if (!isCaller)
        {
            buyButton.gameObject.SetActive(false);
            auctionButton.gameObject.SetActive(false);
        }
        if (propertyToSell is NLand)
        {
            GameObject landCard = Instantiate(landCardPrefab, propertyCardPlaceholder, false);
            landCard.transform.localScale = new Vector3(0.75f, 0.75f, 1);
            landCard.transform.position = propertyCardPlaceholder.position;
            landCard.GetComponent<NDetailedLandCard>().Initialize(propertyToSell as NLand);
            if (propertyToSell.PropertyName == "Vatican")
            {
                buyButton.interactable = false;
            }
        }
        else if (propertyToSell is NRailroad)
        {
            GameObject railroadCard = Instantiate(railroadCardPrefab, propertyCardPlaceholder, false);
            railroadCard.transform.localScale = new Vector3(0.75f, 0.75f, 1);
            railroadCard.transform.position = propertyCardPlaceholder.position;
            railroadCard.GetComponent<NDetailedRailroadCard>().Initialize(propertyToSell as NRailroad);
        }
    }

    
    public void OnBuyButtonClicked()
    {
        NPlayerManager.instance.TryToBuyProperty(_property);
        GetComponent<PhotonView>().RPC("RPC_DestroyDialog", PhotonTargets.All);
    }

    public void OnAuctionButtonClicked()
    {
        NPlayerManager.instance.AuctionProperty(_property);
        GetComponent<PhotonView>().RPC("RPC_DestroyDialog", PhotonTargets.All);
    }

    
}
