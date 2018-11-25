using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railroad : Property {

    int initialRent;

    public void Initialize(string name, int pPrice, int initRent, Color backgroundColor, Color mortgagedBackgroundColor)
    {
        propertyName = name;
        purchasePrice = pPrice;
        initialRent = initRent;
        currentRent = initialRent;

        bgColor = backgroundColor;
        mortgagedBgColor = mortgagedBackgroundColor;
        backgroundSR = GetComponentInChildren<SpriteRenderer>();
    }

    public override void StepOn(Player player)
    {
        UpdateRent();
        base.StepOn(player);
    }

    public void UpdateRent()
    {
        if (owner != null)
        {
            currentRent = (int)Mathf.Pow(2, owner.NumRaildRoads - 1) * initialRent;
        }
    }
}
