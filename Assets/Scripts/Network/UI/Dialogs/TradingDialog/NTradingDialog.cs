using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NTradingDialog : NDialog {

    #region Singleton
    public static NTradingDialog instance;
    private void Awake()
    {
        if (instance != null)
            Debug.Log("Multiple NTradingDialog. Something went wrong");
        instance = this;
    }


    #endregion
    [SerializeField]
    private Text playerNameA;
    [SerializeField]
    private Text playerNameB;
    [SerializeField]
    private Transform propertyListA;
    [SerializeField]
    private Transform propertyListB;
    [SerializeField]
    private Slider moneySliderA;
    [SerializeField]
    private Slider moneySliderB;
    [SerializeField]
    private InputField moneyTextA;
    [SerializeField]
    private InputField moneyTextB;
    [SerializeField]
    private Button offerButton;
    [SerializeField]
    private Button cancelButton;
    [SerializeField]
    private Button acceptButton;
    [SerializeField]
    private Button declineButton;

    public GameObject propertyMiniCardPrefab;
    List<NPropertyMiniCard> propertyMiniCardsA = new List<NPropertyMiniCard>();
    List<NPropertyMiniCard> propertyMiniCardsB = new List<NPropertyMiniCard>();

    NPlayer playerA;
    NPlayer playerB;

    int moneyPlayerAOffers;
    int moneyPlayerBOffers;
    List<int> propertiesPlayerAOffers = new List<int>();
    List<int> propertiesPlayerBOffers = new List<int>();


    // redundant??
    private void OnDestroy()
    {
        instance = null;
    }

    // mode: 0.Trader  1.Tradee  2.Others
    public void Initialize(PhotonPlayer trader, PhotonPlayer tradee, int mode)
    {
        if (mode == 0)
        {
            offerButton.gameObject.SetActive(true);
            cancelButton.gameObject.SetActive(true);
            moneySliderA.interactable = true;
            moneyTextA.interactable = true;
            moneySliderB.interactable = true;
            moneyTextB.interactable = true;
        }
        else if (mode == 1)
        {
            offerButton.gameObject.SetActive(false);
            cancelButton.gameObject.SetActive(false);
            acceptButton.gameObject.SetActive(true);
            declineButton.gameObject.SetActive(true);
        }


        playerA = NPlayerManager.instance.FindGamePlayer(trader);
        playerB = NPlayerManager.instance.FindGamePlayer(tradee);
        
        // set name texts
        playerNameA.text = trader.NickName;
        playerNameB.text = tradee.NickName;

        // set money sliders
        moneySliderA.maxValue = playerA.CurrentMoney;
        moneySliderB.maxValue = playerB.CurrentMoney;

        // set property list panels
        for (int i = 0; i < playerA.Properties.Count; i++)
        {
            GameObject propertyMiniCard = Instantiate(propertyMiniCardPrefab);
            propertyMiniCard.transform.SetParent(propertyListA, false);
            propertyMiniCardsA.Add(propertyMiniCard.GetComponent<NPropertyMiniCard>());
            propertyMiniCard.GetComponent<NPropertyMiniCard>().Initialize(playerA.Properties[i], 0, i, (mode == 0));
        }
        for (int i = 0; i < playerB.Properties.Count; i++)
        {
            GameObject propertyMiniCard = Instantiate(propertyMiniCardPrefab);
            propertyMiniCard.transform.SetParent(propertyListB, false);
            propertyMiniCardsB.Add(propertyMiniCard.GetComponent<NPropertyMiniCard>());
            propertyMiniCard.GetComponent<NPropertyMiniCard>().Initialize(playerB.Properties[i], 1, i, (mode == 0));
        }
    }
    

    public void OnSliderAValueChanged(float value)
    {
        //moneyTextA.text = value.ToString();
    }

    public void OnSliderBValueChanged(float value)
    {
        //moneyTextB.text = value.ToString();
    }

    public void OnMoneyTextAValueChanged(string value)
    {
        if (value == "")
            value = "0";
        int num = int.Parse(value);
        num = Mathf.Clamp(num, 0, playerA.CurrentMoney);

        photonView.RPC("RPC_ReceiveMoneyChange", PhotonTargets.All, 0, num);
    }

    public void OnMoneyTextBValueChanged(string value)
    {
        if (value == "")
            value = "0";
        int num = int.Parse(value);
        num = Mathf.Clamp(num, 0, playerB.CurrentMoney);

        photonView.RPC("RPC_ReceiveMoneyChange", PhotonTargets.All, 1, num);
    }

    [PunRPC]
    public void RPC_ReceiveMoneyChange(int side, int newMoney)
    {
        if (side == 0)
        {
            moneyTextA.text = newMoney.ToString();
        }
        else
        {
            moneyTextB.text = newMoney.ToString();
        }
    }


    public void ToggleProperty(int side, int index, bool isSelected)
    {
        photonView.RPC("RPC_ReceiveToggledProperty", PhotonTargets.All, side, index, isSelected);
    }

    [PunRPC]
    public void RPC_ReceiveToggledProperty(int side, int index, bool isSelected)
    {
        if (side == 0)
        {
            propertyMiniCardsA[index].SetToggle(isSelected);
        }
        else
        {
            propertyMiniCardsB[index].SetToggle(isSelected);
        }
    }

    public void OnOfferButtonClicked()
    {
        offerButton.interactable = false;
        cancelButton.interactable = false;

        photonView.RPC("RPC_MakeTradeOffer", playerB.photonView.owner);
    }

    [PunRPC]
    public void RPC_MakeTradeOffer()
    {
        acceptButton.interactable = true;
        declineButton.interactable = true;
    }


    public void OnCancelButtonClicked()
    {
        photonView.RPC("RPC_DestroyDialog", PhotonTargets.All);
    }

    public void OnAcceptButtonClicked()
    {
        moneyPlayerAOffers = int.Parse(moneyTextA.text);
        moneyPlayerBOffers = int.Parse(moneyTextB.text);

        foreach (NPropertyMiniCard pmc in propertyMiniCardsA)
        {
            if (pmc.IsSelected())
                propertiesPlayerAOffers.Add(pmc.Property.PropertyID);
        }
        foreach (NPropertyMiniCard pmc in propertyMiniCardsB)
        {
            if (pmc.IsSelected())
                propertiesPlayerBOffers.Add(pmc.Property.PropertyID);
        }

        int[] propertyIDs_A = propertiesPlayerAOffers.ToArray();
        int[] propertyIDs_B = propertiesPlayerBOffers.ToArray();

        NPlayerManager.instance.TradeProperty(playerA.photonView.owner, playerB.photonView.owner, propertyIDs_A, propertyIDs_B, moneyPlayerAOffers, moneyPlayerBOffers);

        photonView.RPC("RPC_DestroyDialog", PhotonTargets.All);
    }

    public void OnDeclineButtonClicked()
    {
        photonView.RPC("RPC_DestroyDialog", PhotonTargets.All);
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
