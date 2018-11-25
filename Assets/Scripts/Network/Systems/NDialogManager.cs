using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NDialogManager : Photon.MonoBehaviour {

    #region Singleton
    public static NDialogManager instance;
    private void Awake()
    {
        if (instance != null)
            Debug.Log("Multiple PropertyPurchaseHandler. Something went wrong");
        instance = this;
    }
    #endregion

    [SerializeField]
    private GameObject propertyPurchaseDialogPrefab;
    [SerializeField]
    private GameObject inJailDialogPrefab;
    [SerializeField]
    private GameObject tradingDialogPrefab;
    [SerializeField]
    private GameObject auctionDialogPrefab;

    List<NProperty> _properties;


    public void Initialize()
    {
        _properties = NBoardManager.instance.Properties;
    }

    public void CallPropertyPurchaseDialog(int propertyIndex, PhotonPlayer caller)
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        int id = PhotonNetwork.AllocateViewID();
        photonView.RPC("RPC_CreatePropertyPurchaseDialog", PhotonTargets.All, propertyIndex, caller, id);
    }

    [PunRPC]
    public void RPC_CreatePropertyPurchaseDialog(int propertyIndex, PhotonPlayer caller, int viewID)
    {
        GameObject dialogObj = Instantiate(propertyPurchaseDialogPrefab);
        dialogObj.GetComponent<PhotonView>().viewID = viewID;
        dialogObj.GetComponent<NPropertyPurchaseDialog>().Initialize(_properties[propertyIndex], (caller == PhotonNetwork.player));
    }

    /*
    public AuctionDialog CallAuctionDialog(NPlayer callingPlayer, NProperty propertyToAuction)
    {
        GameObject dialog = Instantiate(auctionDialogPrefab);
        AuctionDialog ad = dialog.GetComponent<AuctionDialog>();
        ad.Initialize(callingPlayer, propertyToAuction);
        return ad;
    }

    public void CallInJailDialog(NPlayer player)
    {
        Instantiate(inJailDialogPrefab);
    }
    */
    public void CallTradingDialog(NPlayer callingPlayer)
    {
        GameObject dialog = Instantiate(tradingDialogPrefab);
        dialog.GetComponent<NTradingDialog>().Initialize(callingPlayer);
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
