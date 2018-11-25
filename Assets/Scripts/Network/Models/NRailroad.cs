using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NRailroad : NProperty {

    int _initialRent;

    public void Initialize(string name, int id, int pPrice, int initRent, Color backgroundColor, Color mortgagedBackgroundColor)
    {
        _propertyName = name;
        _propertyID = id;
        _purchasePrice = pPrice;
        _initialRent = initRent;
        _currentRent = initRent;

        bgColor = backgroundColor;
        mortgagedBgColor = mortgagedBackgroundColor;
        backgroundSR = GetComponentInChildren<SpriteRenderer>();
    }

    public override void StepOn(NPlayer player)
    {
        base.StepOn(player);
        UpdateRent();
    }

    public void UpdateRent()
    {
        if (_owner != null)
        {
            _currentRent = (int)Mathf.Pow(2, _owner.NumRaildRoads - 1) * _initialRent;
        }
    }
}
