using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailedRailroadCard : MonoBehaviour {

    public Text TextRailroadName;
    public Text TextPurchasePrice;

    public void Initialize(Railroad railroad)
    {
        TextRailroadName.text = railroad.PropertyName;
        TextPurchasePrice.text = "$" + railroad.PurchasePrice.ToString();
    }
}
