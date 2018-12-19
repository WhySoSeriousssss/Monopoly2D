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
    private GameObject selectTradeeDialogPrefab;
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

    public void CallSelectTradeeDialog(PhotonPlayer caller)
    {
        GameObject dialog = Instantiate(selectTradeeDialogPrefab);
        dialog.GetComponentInChildren<NSelectTradeeDialog>().Initialize(caller);
    }


    public void CallTradingDialog(PhotonPlayer trader, PhotonPlayer tradee)
    {
        photonView.RPC("RPC_CallTradingDialog", PhotonTargets.MasterClient, trader, tradee);
    }

    [PunRPC]
    public void RPC_CallTradingDialog(PhotonPlayer trader, PhotonPlayer tradee)
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        int id = PhotonNetwork.AllocateViewID();
        photonView.RPC("RPC_CreateTradingDialog", PhotonTargets.All, trader, tradee, id);

    }

    [PunRPC]
    public void RPC_CreateTradingDialog(PhotonPlayer trader, PhotonPlayer tradee, int viewID)
    {
        int mode;
        if (PhotonNetwork.player == trader)
            mode = 0;
        else if (PhotonNetwork.player == tradee)
            mode = 1;
        else
            mode = 2;

        GameObject dialogObj = Instantiate(tradingDialogPrefab);
        dialogObj.GetComponentInChildren<PhotonView>().viewID = viewID;
        dialogObj.GetComponentInChildren<NTradingDialog>().Initialize(trader, tradee, mode);
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
