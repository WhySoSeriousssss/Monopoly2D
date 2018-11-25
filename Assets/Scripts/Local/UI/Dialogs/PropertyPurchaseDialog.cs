using UnityEngine;
using UnityEngine.UI;

public class PropertyPurchaseDialog : MonoBehaviour {

    public GameObject landCardPrefab;
    public GameObject railroadCardPrefab;
    public Transform propertyCardPlaceholder;
    public Button buyButton;

    Property property;

    public void Initialize(Property propertyToSell)
    {
        property = propertyToSell;
        if (propertyToSell is Land)
        {
            GameObject landCard = Instantiate(landCardPrefab, propertyCardPlaceholder, false);
            landCard.transform.localScale = new Vector3(0.75f, 0.75f, 1);
            landCard.transform.position = propertyCardPlaceholder.position;
            landCard.GetComponent<DetailedLandCard>().Initialize(propertyToSell as Land);
            if (propertyToSell.PropertyName == "Vatican")
            {
                buyButton.interactable = false;
            }
        }
        else  if (propertyToSell is Railroad)
        {
            GameObject railroadCard = Instantiate(railroadCardPrefab, propertyCardPlaceholder, false);
            railroadCard.transform.localScale = new Vector3(0.75f, 0.75f, 1);
            railroadCard.transform.position = propertyCardPlaceholder.position;
            railroadCard.GetComponent<DetailedRailroadCard>().Initialize(propertyToSell as Railroad);
        }
    }

	public void HandleBuyButtonOnClick()
    {
        LocPlayerController.instance.TryingToBuyProperty(property);
        Destroy(gameObject);
    }

    public void HandleAuctionButtonOnClick()
    {
        LocPlayerController.instance.AuctionProperty(property);
        Destroy(gameObject);
    }
}
