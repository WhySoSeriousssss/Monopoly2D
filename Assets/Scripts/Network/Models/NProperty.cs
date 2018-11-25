using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NProperty : NSpace {

    protected string _propertyName;
    public string PropertyName { get { return _propertyName; } }

    protected int _propertyID;
    public int PropertyID { get { return _propertyID; } }

    protected int _purchasePrice;
    public int PurchasePrice { get { return _purchasePrice; } }

    protected int _currentRent;

    protected NPlayer _owner;
    public NPlayer Owner { get { return _owner; } set { _owner = value; } }

    protected Vector3 _ownerMarkerPos;
    public Vector3 OwnerMarkerPos { get { return _ownerMarkerPos; } set { _ownerMarkerPos = value; } }

    private SpriteRenderer _ownerMarkerSR = null;
    public SpriteRenderer OwnerMarkerSR { get { return _ownerMarkerSR; } set { _ownerMarkerSR = value; } }

    protected Color bgColor;
    protected Color mortgagedBgColor;
    protected SpriteRenderer backgroundSR;

    public delegate void OnOwnerChanged();
    public OnOwnerChanged OnOwnerChangedCallBack;



    public override void StepOn(NPlayer player)
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        if (_owner == null)
        {
            NDialogManager.instance.CallPropertyPurchaseDialog(_propertyID, player.photonView.owner);
        }
        else
        {
            if (_owner != player)
            {
                _owner.ChangeMoney(_currentRent);
                player.ChangeMoney(-_currentRent);
                Debug.Log(player.photonView.owner.NickName + " paid $" + _currentRent + " to " + _owner.photonView.owner.NickName);
            }
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
