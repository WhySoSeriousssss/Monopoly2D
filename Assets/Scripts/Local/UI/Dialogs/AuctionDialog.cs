using UnityEngine;
using UnityEngine.UI;

public class AuctionDialog : MonoBehaviour {

    public InputField bidPriceText;
    public Text bidInfoText;
    public Text warningMessageText;

    public Transform propertyCardPlaceholder;
    public GameObject landCardPrefab;
    public GameObject railroadCardPrefab;
    
    int currentBidPrice;

    Property property;

	public void Initialize(Player callingPlayer, Property propertyToAuction)
    {
        // set up the property card
        if (propertyToAuction is Land)
        {
            GameObject landCard = Instantiate(landCardPrefab, propertyCardPlaceholder, false);
            landCard.transform.localScale = new Vector3(0.9f, 0.9f, 1);
            landCard.transform.position = propertyCardPlaceholder.position;
            landCard.GetComponent<DetailedLandCard>().Initialize(propertyToAuction as Land);
        }
        else if (propertyToAuction is Railroad)
        {
            GameObject railroadCard = Instantiate(railroadCardPrefab, propertyCardPlaceholder, false);
            railroadCard.transform.localScale = new Vector3(0.9f, 0.9f, 1);
            railroadCard.transform.position = propertyCardPlaceholder.position;
            railroadCard.GetComponent<DetailedRailroadCard>().Initialize(propertyToAuction as Railroad);
        }

        currentBidPrice = propertyToAuction.PurchasePrice;
        property = propertyToAuction;
        bidInfoText.text = callingPlayer.PlayerName + " has started the auction\n";
        bidPriceText.text = currentBidPrice.ToString();
    }

    public void BidButtonOnClick()
    {
        int newBidPrice = int.Parse(bidPriceText.text);
        if (newBidPrice >= (currentBidPrice + 10))
        {
            warningMessageText.text = "";
            AuctionSystem.instance.ReceiveBid(true, newBidPrice);
        }
        else
        {
            warningMessageText.text = "New bid must be at least $10 higher than the previous one!";
        }
    }

    public void StopButtonOnClick()
    {
        warningMessageText.text = "";
        AuctionSystem.instance.ReceiveBid(false, 0);
    }

    public void AuctionFinish()
    {
        Destroy(gameObject);
    }

    public void UpdateNewBid(Player player, int bidPrice)
    {
        currentBidPrice = bidPrice;
        bidInfoText.text += player.PlayerName + " Bidded $" + bidPrice.ToString() + "\n";
        bidPriceText.text = bidPrice.ToString();

        bidInfoText.rectTransform.offsetMin = new Vector2(bidInfoText.rectTransform.offsetMin.x, bidInfoText.rectTransform.offsetMin.y - 30);
    }

    public void UpdateStopBid(Player player)
    {
        bidInfoText.text += player.PlayerName + " gave up bidding.\n";

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
}
