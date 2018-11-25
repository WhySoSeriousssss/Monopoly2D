using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NDetailedRailroadCard : MonoBehaviour {

    [SerializeField]
    private Text textRailroadName;
    [SerializeField]
    private Text textPurchasePrice;

    public void Initialize(NRailroad railroad)
    {
        textRailroadName.text = railroad.PropertyName;
        textPurchasePrice.text = "$" + railroad.PurchasePrice.ToString();
    }
}
