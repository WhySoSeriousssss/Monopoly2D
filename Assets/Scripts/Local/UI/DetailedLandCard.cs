using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailedLandCard : MonoBehaviour {

    public Image TopBar;
    public Text TextLandName;
    public Text[] TextRents;
    public Text TextUpgradePrice;
    public Text TextPurchasePrice;

    public void Initialize(Land land)
    {
        TopBar.color = land.Group;
        TextLandName.text = land.PropertyName;
        for (int i = 0; i <= Land.maxLevel; i++)
        {
            TextRents[i].text = "$" + land.Rents[i].ToString();
        }
        TextUpgradePrice.text = "$" + land.UpgradePrice.ToString();
        TextPurchasePrice.text = "$" + land.PurchasePrice.ToString();
    }
}
