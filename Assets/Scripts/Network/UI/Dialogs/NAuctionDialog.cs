using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NAuctionDialog : NDialog {

    [SerializeField]
    private InputField bidPriceText;
    [SerializeField]
    private Text bidInfoText;
    [SerializeField]
    private Text warningMessageText;
    [SerializeField]
    private Transform propertyCardPlaceholder;
    [SerializeField]
    private GameObject landCardPrefab;
    [SerializeField]
    private GameObject railroadCardPrefab;
    [SerializeField]
    private Button bidButton;
    [SerializeField]
    private Button giveupButton;

    int _currentBid;

    NProperty _property;


    public void Initialize(NPlayer caller, NProperty property)
    {
        // set up the property card
        if (property is NLand)
        {
            GameObject landCard = Instantiate(landCardPrefab, propertyCardPlaceholder, false);
            landCard.transform.localScale = new Vector3(0.9f, 0.9f, 1);
            landCard.transform.position = propertyCardPlaceholder.position;
            landCard.GetComponent<NDetailedLandCard>().Initialize(property as NLand);
        }
        else if (property is NRailroad)
        {
            GameObject railroadCard = Instantiate(railroadCardPrefab, propertyCardPlaceholder, false);
            railroadCard.transform.localScale = new Vector3(0.9f, 0.9f, 1);
            railroadCard.transform.position = propertyCardPlaceholder.position;
            railroadCard.GetComponent<NDetailedRailroadCard>().Initialize(property as NRailroad);
        }

        _currentBid = property.PurchasePrice;
        _property = property;
        bidInfoText.text = caller.photonView.owner.NickName + " has started the auction\n";
        bidPriceText.text = _currentBid.ToString();

        NAuctionManager.instance.Initialize(caller, property, this);
    }


    public void StartBid()
    {
        bidButton.interactable = true;
        giveupButton.interactable = true;
    }

    public void FinishBid()
    {
        bidButton.interactable = false;
        giveupButton.interactable = false;
    }


    public void OnBidButtonClicked()
    {
        int newBidPrice = int.Parse(bidPriceText.text);
        if (newBidPrice >= (_currentBid + 10))
        {
            warningMessageText.text = "";
            NAuctionManager.instance.SendBid(true, newBidPrice);
            NAuctionManager.instance.FinishBid = true;
        }
        else
        {
            warningMessageText.text = "New bid must be at least $10 higher than the previous one!";
        }
    }

    public void OnGiveUpButtonClicked()
    {
        warningMessageText.text = "";
        NAuctionManager.instance.SendBid(false, 0);
        NAuctionManager.instance.FinishBid = true;
    }

    public void AuctionFinish()
    {
        Destroy(gameObject);
    }


    // Update new bid info by players
    public void UpdateNewBid(string playerName, int bidPrice)
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        photonView.RPC("RPC_UpdateNewBid", PhotonTargets.All, playerName, bidPrice);
    }

    [PunRPC]
    public void RPC_UpdateNewBid(string playerName, int bidPrice)
    {
        _currentBid = bidPrice;
        bidInfoText.text += playerName + " Bidded $" + bidPrice.ToString() + "\n";
        bidPriceText.text = bidPrice.ToString();

        bidInfoText.rectTransform.offsetMin = new Vector2(bidInfoText.rectTransform.offsetMin.x, bidInfoText.rectTransform.offsetMin.y - 30);
    }

    // Update Giveup-bid info by players
    public void UpdateGiveUpBid(string playerName)
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        photonView.RPC("RPC_UpdateGiveUpBid", PhotonTargets.All, playerName);
    }

    [PunRPC]
    public void RPC_UpdateGiveUpBid(string playerName)
    {
        bidInfoText.text += playerName + " gave up bidding.\n";

        bidInfoText.rectTransform.offsetMin = new Vector2(bidInfoText.rectTransform.offsetMin.x, bidInfoText.rectTransform.offsetMin.y - 30);
    }



    public void AddTenButtonOnClicked()
    {
        bidPriceText.text = (int.Parse(bidPriceText.text) + 10).ToString();
    }

    public void AddAHundredButtonOnClicked()
    {
        bidPriceText.text = (int.Parse(bidPriceText.text) + 100).ToString();
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
