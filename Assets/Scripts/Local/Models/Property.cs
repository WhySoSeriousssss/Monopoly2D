using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property : Space {

    protected string propertyName;
    public string PropertyName { get { return propertyName; } }

    protected int purchasePrice;
    public int PurchasePrice { get { return purchasePrice; } }

    protected int currentRent;

    protected bool isMortgaged = false;
    public bool IsMortgaged { get { return isMortgaged; } set { isMortgaged = value; } }

    protected Player owner;
    public Player Owner { get { return owner; } set { owner = value; } }

    protected Vector3 ownerMarkerPos;
    public Vector3 OwnerMarkerPos { set { ownerMarkerPos = value; } }

    SpriteRenderer ownerMarkerSR = null;

    protected Color bgColor;
    protected Color mortgagedBgColor;
    protected SpriteRenderer backgroundSR;

    public delegate void OnOwnerChanged();
    public OnOwnerChanged OnOwnerChangedCallBack;


    public override void StepOn(Player player)
    {
        if (owner == null)
        {
            // pop up window asking whether player wants to buy this property
            DialogHandler.instance.CallPropertyPurchaseWindow(this);
        }
        else
        {
            if (owner != player && !isMortgaged && !owner.IsInJail)
            {
                // Pay the rent
                player.LoseMoney(currentRent);
                owner.GetMoney(currentRent);
                Debug.Log(player.PlayerName + " paid " + currentRent + " to " + owner.PlayerName);
            }
        }
    }

    // change the owner to newOwner, remove the old marker, create the new marker
    public virtual void SoldTo(Player newOwner)
    {
        owner = newOwner;
        RemoveOwnerMarker();
        CreateOwnerMarker();
    }

    void CreateOwnerMarker()
    {
        if (owner != null)
        {
            ownerMarkerSR = Instantiate(owner.SR, ownerMarkerPos, Quaternion.identity);
            ownerMarkerSR.transform.localScale = new Vector3(0.2f, 0.2f);
        }
    }

    void RemoveOwnerMarker()
    {
        if (ownerMarkerSR != null)
        {
            Destroy(ownerMarkerSR);
            ownerMarkerSR = null;
        }
    }

    public void ToggleMortgagedBackground()
    {
        if (backgroundSR.color == bgColor)
            backgroundSR.color = mortgagedBgColor;
        else
            backgroundSR.color = bgColor;
    }
}
