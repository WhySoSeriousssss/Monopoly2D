using UnityEngine;
using UnityEngine.UI;

public class NAuctionDialog : NDialog {

    [SerializeField]
    private InputField bidInput;
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

    bool _isCaller;


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

        _isCaller = (caller == NPlayer.thisPlayer);
        _currentBid = property.PurchasePrice;
        bidInfoText.text = caller.photonView.owner.NickName + " has started the auction\n";
        bidInput.text = _currentBid.ToString();

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
        int newBid = int.Parse(bidInput.text);
        if (newBid >= (_currentBid + 10) || (newBid >= _currentBid && _isCaller))
        {
            warningMessageText.text = "";
            NAuctionManager.instance.SendBid(true, newBid);
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


    public void OnBidInputValueChanged(string newValue)
    {
        int num = int.Parse(newValue);
        num = Mathf.Clamp(num, 0, NPlayer.thisPlayer.CurrentMoney);
        bidInput.text = num.ToString();
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
    public void RPC_UpdateNewBid(string playerName, int bid)
    {
        _currentBid = bid;
        bidInfoText.text += playerName + " Bidded $" + bid.ToString() + "\n";
        bidInput.text = bid.ToString();

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



    public void OnAddTenButtonClicked()
    {
        if (int.Parse(bidInput.text) + 10 <= NPlayer.thisPlayer.CurrentMoney)
            bidInput.text = (int.Parse(bidInput.text) + 10).ToString();
    }

    public void OnAddAHundredButtonClicked()
    {
        if (int.Parse(bidInput.text) + 100 <= NPlayer.thisPlayer.CurrentMoney)
            bidInput.text = (int.Parse(bidInput.text) + 100).ToString();
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
