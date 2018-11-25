using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradingPanel : MonoBehaviour {

    public Text playerNameA;
    public Text playerNameB;
    public Transform propertyListA;
    public Transform propertyListB;
    public Slider moneySliderA;
    public Slider moneySliderB;
    public InputField moneyTextA;
    public InputField moneyTextB;
    public Button offerButton;
    public Button cancelButton;

    public GameObject propertyMiniCardPrefab;
    List<PropertyMiniCard> propertyMiniCardsA = new List<PropertyMiniCard>();
    List<PropertyMiniCard> propertyMiniCardsB = new List<PropertyMiniCard>();

    Player playerA;
    Player playerB;

    int moneyPlayerAOffers;
    int moneyPlayerBOffers;
    List<Property> propertiesPlayerAOffers = new List<Property>();
    List<Property> propertiesPlayerBOffers = new List<Property>();

    public void Initialize(Player callingPlayer, Player tradedPlayer)
    {
        playerA = callingPlayer;
        playerB = tradedPlayer;

        // set name texts
        playerNameA.text = playerA.PlayerName;
        playerNameB.text = playerB.PlayerName;

        // set money sliders
        moneySliderA.maxValue = playerA.CurrentMoney;
        moneySliderB.maxValue = playerB.CurrentMoney;

        // set property list panels
        foreach(Property prop in playerA.Properties)
        {
            GameObject propertyMiniCard = Instantiate(propertyMiniCardPrefab);
            propertyMiniCard.transform.SetParent(propertyListA, false);
            propertyMiniCardsA.Add(propertyMiniCard.GetComponent<PropertyMiniCard>());
            propertyMiniCard.GetComponent<PropertyMiniCard>().Initialize(prop);
        }
        foreach (Property prop in playerB.Properties)
        {
            GameObject propertyMiniCard = Instantiate(propertyMiniCardPrefab);
            propertyMiniCard.transform.SetParent(propertyListB, false);
            propertyMiniCardsB.Add(propertyMiniCard.GetComponent<PropertyMiniCard>());
            propertyMiniCard.GetComponent<PropertyMiniCard>().Initialize(prop);
        }
    }

    public void OnSliderAValueChanged(float value)
    {
        moneyTextA.text = value.ToString();
    }

    public void OnSliderBValueChanged(float value)
    {
        moneyTextB.text = value.ToString();
    }

    public void OnMoneyTextAValueChanged(string value)
    {
        if (value == "")
            value = "0";
        int num = int.Parse(value);
        num = Mathf.Clamp(num, 0, playerA.CurrentMoney);

        moneyTextA.text = num.ToString();
        moneySliderA.value = num;
    }

    public void OnMoneyTextBValueChanged(string value)
    {
        if (value == "")
            value = "0";
        int num = int.Parse(value);
        num = Mathf.Clamp(num, 0, playerB.CurrentMoney);

        moneyTextB.text = num.ToString();
        moneySliderB.value = num;
    }

    public void OfferButtonOnClick()
    {
        moneyPlayerAOffers = int.Parse(moneyTextA.text);
        moneyPlayerBOffers = int.Parse(moneyTextB.text);

        foreach(PropertyMiniCard pmc in propertyMiniCardsA)
        {
            if (pmc.IsSelected())
                propertiesPlayerAOffers.Add(pmc.Property);
        }
        foreach (PropertyMiniCard pmc in propertyMiniCardsB)
        {
            if (pmc.IsSelected())
                propertiesPlayerBOffers.Add(pmc.Property);
        }

        playerA.Exchange(moneyPlayerAOffers, propertiesPlayerAOffers, moneyPlayerBOffers, propertiesPlayerBOffers);
        playerB.Exchange(moneyPlayerBOffers, propertiesPlayerBOffers, moneyPlayerAOffers, propertiesPlayerAOffers);

        Destroy(GetComponentInParent<TradingDialog>().gameObject);
    }

    public void CancelButtonOnClick()
    {
        Destroy(GetComponentInParent<TradingDialog>().gameObject);
    }
}
