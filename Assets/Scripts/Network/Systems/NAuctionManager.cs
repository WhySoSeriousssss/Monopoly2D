using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAuctionManager : Photon.MonoBehaviour {

    #region Singleton
    public static NAuctionManager instance;
    private void Awake()
    {
        if (instance != null)
            Debug.Log("Multiple NAuctionManager. Something went wrong");
        instance = this;
    }
    #endregion

    // master client side
    List<NPlayer> _bidders;
    
    int _currentBidder;
    bool _bidReceived = false;
    bool _bidOrGiveup;
    int _currentBid;

    // regular client side
    int _numBidders;

    bool _canBid;

    private bool _finishBid;
    public bool FinishBid { get { return _finishBid; } set { _finishBid = value; } }

    NProperty _property;

    NAuctionDialog _auctionDialog;

    

    public void Initialize(NPlayer startingPlayer, NProperty property, NAuctionDialog auctionDialog)
    {
        _bidders = new List<NPlayer>(NGameplay.instance.Players);
        _property = property;
        _currentBidder = startingPlayer.Order;
        _currentBid = property.PurchasePrice;
        _auctionDialog = auctionDialog;
        photonView.RPC("RPC_UpdateNumBidder", PhotonTargets.All, _bidders.Count);

        StartCoroutine(Bidding());
    }

    IEnumerator Bidding()
    {
        while (_numBidders > 1)
        {
            if (PhotonNetwork.isMasterClient)
            {
                photonView.RPC("RPC_SendCanBid", _bidders[_currentBidder].photonView.owner);

                if(_currentBidder == NPlayer.thisPlayer.Order)
                {
                    _auctionDialog.StartBid();

                    while (!_finishBid)
                        yield return null;

                    _auctionDialog.FinishBid();
                    _canBid = false;
                    _finishBid = false;
                }

                while (!_bidReceived)
                    yield return null;
                _bidReceived = false;

                if (_bidOrGiveup)
                {
                    _auctionDialog.UpdateNewBid(_bidders[_currentBidder].photonView.owner.NickName, _currentBid);
                    _currentBidder = (_currentBidder + 1) % _bidders.Count;
                }
                else
                {
                    _auctionDialog.UpdateGiveUpBid(_bidders[_currentBidder].photonView.owner.NickName);
                    _bidders.Remove(_bidders[_currentBidder]);
                    photonView.RPC("RPC_UpdateNumBidder", PhotonTargets.All, _bidders.Count);
                    if (_currentBidder == _bidders.Count)
                        _currentBidder = 0;
                }
            }
            else
            {
                while (!_canBid)
                    yield return null;
                _auctionDialog.StartBid();

                while (!_finishBid)
                    yield return null;

                _auctionDialog.FinishBid();
                _canBid = false;
                _finishBid = false;
            }

        }
        //LocPlayerController.instance.WonAuction(playersInAuction[0], property, currentBiddingPrice);
        _auctionDialog.AuctionFinish();
    }

    
    public void SendBid(bool bidOrNot, int newBid)
    {
        if (_canBid)
            photonView.RPC("RPC_ReceiveBid", PhotonTargets.MasterClient, bidOrNot, newBid);
    }

    [PunRPC]
    public void RPC_ReceiveBid(bool bidOrNot, int newBid)
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        _bidReceived = true;
        _bidOrGiveup = bidOrNot;
        if (_bidOrGiveup)
            _currentBid = newBid;
        Debug.Log("Bid received::$" + newBid);
    }

    // Called by MasterClient to others to set up the canBid flag
    [PunRPC]
    public void RPC_SendCanBid()
    {
        _canBid = true;
    }


    [PunRPC]
    public void RPC_UpdateNumBidder(int newNum)
    {
        _numBidders = newNum;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
