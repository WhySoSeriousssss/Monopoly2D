using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NDetailedLandCard : MonoBehaviour {

    [SerializeField]
    private Image topBar;
    [SerializeField]
    private Text textLandName;
    [SerializeField]
    private Text[] textRents;
    [SerializeField]
    private Text textUpgradePrice;
    [SerializeField]
    private Text textPurchasePrice;

    public void Initialize(NLand land)
    {
        topBar.color = land.Group;
        textLandName.text = land.PropertyName;
        for (int i = 0; i <= Land.maxLevel; i++)
        {
            textRents[i].text = "$" + land.Rents[i].ToString();
        }
        textUpgradePrice.text = "$" + land.UpgradePrice.ToString();
        textPurchasePrice.text = "$" + land.PurchasePrice.ToString();
    }
}
